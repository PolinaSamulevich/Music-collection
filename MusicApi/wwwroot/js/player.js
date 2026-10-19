(() => {
    'use strict';

    // ---------- элементы страницы ----------
    const $ = (id) => document.getElementById(id);
    const el = {
        list: $('track-list'),
        genres: $('genres'),
        search: $('search'),
        summary: $('summary'),
        player: $('player'),
        nowCover: $('now-cover'),
        nowTitle: $('now-title'),
        nowArtist: $('now-artist'),
        btnPlay: $('btn-play'),
        btnPrev: $('btn-prev'),
        btnNext: $('btn-next'),
        seek: $('seek'),
        volume: $('volume'),
        timeNow: $('time-now'),
        timeTotal: $('time-total')
    };

    // ---------- состояние ----------
    const audio = new Audio();
    audio.preload = 'metadata';

    let tracks = [];       // все треки с сервера
    let visible = [];      // то, что показано сейчас (после поиска и жанра)
    let queue = [];        // очередь: список, из которого запустили трек
    let current = -1;      // индекс играющего трека в queue
    let genre = 'Все';
    let query = '';

    const PLAY_ICON = '<svg viewBox="0 0 24 24" width="20" height="20"><path fill="currentColor" d="M8 5v14l11-7z"/></svg>';

    // ---------- вспомогательные функции ----------
    function esc(s) {
        return String(s ?? '').replace(/[&<>"']/g, (c) =>
            ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
    }

    function fmt(sec) {
        if (!Number.isFinite(sec) || sec < 0) return '0:00';
        const m = Math.floor(sec / 60);
        const s = Math.floor(sec % 60);
        return `${m}:${String(s).padStart(2, '0')}`;
    }

    // Пока у треков нет обложек, рисуем цветную заглушку: цвет зависит от названия.
    function placeholderStyle(title) {
        let h = 0;
        for (const ch of String(title)) h = (h * 31 + ch.codePointAt(0)) % 360;
        return `background:linear-gradient(135deg,hsl(${h} 65% 55%),hsl(${(h + 50) % 360} 65% 42%))`;
    }

    // Обложка: картинка, если у трека есть coverUrl, иначе цветная плитка с первой буквой.
    function coverInner(t) {
        if (t.coverUrl) return `<img src="${esc(t.coverUrl)}" alt="">`;
        return esc(String(t.title).trim().charAt(0).toUpperCase() || '♪');
    }

    function coverStyle(t) {
        return t.coverUrl ? '' : placeholderStyle(t.title);
    }

    function coverHtml(t, cls) {
        return `<div class="${cls}" style="${coverStyle(t)}">${coverInner(t)}</div>`;
    }

    function hasAudio(t) {
        return Boolean(t.audioUrl);
    }

    function currentTrack() {
        return current >= 0 ? queue[current] : null;
    }

    function setRange(input, fraction) {
        input.style.setProperty('--p', `${Math.max(0, Math.min(1, fraction)) * 100}%`);
    }

    function store(key, value) {
        try { localStorage.setItem(key, value); } catch { /* хранилище недоступно, не страшно */ }
    }

    function load(key) {
        try { return localStorage.getItem(key); } catch { return null; }
    }

    // ---------- загрузка данных ----------
    async function loadTracks() {
        el.summary.textContent = 'Загрузка…';
        try {
            const res = await fetch('/tracks', { headers: { Accept: 'application/json' } });
            if (!res.ok) throw new Error(`Сервер ответил ${res.status}`);
            tracks = await res.json();
        } catch (err) {
            tracks = [];
            el.summary.textContent = '';
            el.genres.innerHTML = '';
            el.list.innerHTML =
                `<div class="empty"><strong>Не удалось загрузить треки</strong>` +
                `${esc(err.message)}. Проверь, что запущен SQL Server и база musicdb доступна.</div>`;
            return;
        }
        buildGenres();
        applyFilters();
    }

    // ---------- жанры, поиск, список ----------
    function buildGenres() {
        const names = [...new Set(tracks.map((t) => t.genre).filter(Boolean))].sort((a, b) => a.localeCompare(b));
        const all = ['Все', ...names];
        el.genres.innerHTML = all
            .map((g) => `<button type="button" class="chip${g === genre ? ' is-active' : ''}" data-genre="${esc(g)}">${esc(g)}</button>`)
            .join('');
    }

    function applyFilters() {
        const q = query.trim().toLowerCase();
        visible = tracks.filter((t) => {
            if (genre !== 'Все' && t.genre !== genre) return false;
            if (!q) return true;
            return `${t.title} ${t.artist}`.toLowerCase().includes(q);
        });
        renderList();
    }

    function renderList() {
        const playing = currentTrack();

        el.summary.textContent = tracks.length
            ? (visible.length === tracks.length
                ? `${tracks.length} ${plural(tracks.length, 'трек', 'трека', 'треков')}`
                : `Найдено ${visible.length} из ${tracks.length}`)
            : '';

        if (!visible.length) {
            el.list.innerHTML = tracks.length
                ? '<div class="empty"><strong>Ничего не найдено</strong>Попробуй другой запрос или жанр.</div>'
                : '<div class="empty"><strong>Пока нет треков</strong>Добавь их через <a href="/admin.html" style="color:var(--accent)">админку</a>.</div>';
            return;
        }

        el.list.innerHTML = visible
            .map((t, i) => {
                const isCurrent = playing && playing.id === t.id;
                const classes = ['row'];
                if (isCurrent) classes.push('is-current');
                if (isCurrent && !audio.paused) classes.push('is-playing');
                if (!hasAudio(t)) classes.push('is-disabled');
                return `
<button type="button" class="${classes.join(' ')}" data-id="${t.id}"${hasAudio(t) ? '' : ' disabled title="У этого трека нет аудиофайла"'}>
    <span class="row-num"><span class="num">${i + 1}</span><span class="bars"><i></i><i></i><i></i></span>${PLAY_ICON}</span>
    <span class="row-main">
        ${coverHtml(t, 'cover')}
        <span class="row-text">
            <span class="row-title">${esc(t.title)}</span>
            <span class="row-artist">${esc(t.artist)}</span>
        </span>
    </span>
    <span class="row-genre">${esc(t.genre)}</span>
    <span class="row-time">${esc(t.duration)}</span>
</button>`;
            })
            .join('');
    }

    function plural(n, one, few, many) {
        const m10 = n % 10;
        const m100 = n % 100;
        if (m10 === 1 && m100 !== 11) return one;
        if (m10 >= 2 && m10 <= 4 && (m100 < 12 || m100 > 14)) return few;
        return many;
    }

    // ---------- воспроизведение ----------
    function playFromList(id) {
        const idx = visible.findIndex((t) => t.id === id);
        if (idx === -1) return;
        const track = visible[idx];

        // тот же трек: просто пауза / продолжить
        const playing = currentTrack();
        if (playing && playing.id === track.id) {
            togglePlay();
            return;
        }

        queue = visible.slice();
        current = idx;
        startCurrent();
    }

    function startCurrent() {
        const t = currentTrack();
        if (!t || !hasAudio(t)) return;

        audio.src = t.audioUrl;
        audio.play().catch(() => {
            el.nowArtist.textContent = 'Не удалось воспроизвести этот файл';
        });
        updateNowPlaying(t);
        document.title = `${t.title} — ${t.artist}`;
    }

    function updateNowPlaying(t) {
        el.nowTitle.textContent = t.title;
        el.nowArtist.textContent = t.artist;
        el.nowCover.style.cssText = coverStyle(t);
        el.nowCover.innerHTML = coverInner(t);
        el.timeTotal.textContent = t.duration || '0:00';
        el.seek.value = 0;
        setRange(el.seek, 0);
        el.timeNow.textContent = '0:00';
    }

    function togglePlay() {
        if (!currentTrack()) {
            // ничего не выбрано: запускаем первый трек из списка
            const first = visible.find(hasAudio);
            if (first) playFromList(first.id);
            return;
        }
        if (audio.paused) audio.play().catch(() => {});
        else audio.pause();
    }

    // Следующий трек с аудиофайлом. dir = 1 вперёд, -1 назад. В конце списка идём по кругу.
    function step(dir) {
        if (!queue.length) return;
        for (let n = 1; n <= queue.length; n++) {
            const idx = (((current + dir * n) % queue.length) + queue.length) % queue.length;
            if (hasAudio(queue[idx])) {
                current = idx;
                startCurrent();
                return;
            }
        }
    }

    function prev() {
        // как в Spotify: если трек играет больше 3 секунд, сначала перематываем в начало
        if (audio.currentTime > 3) {
            audio.currentTime = 0;
            return;
        }
        step(-1);
    }

    // ---------- события аудио ----------
    audio.addEventListener('play', syncPlayState);
    audio.addEventListener('pause', syncPlayState);
    audio.addEventListener('ended', () => step(1));

    audio.addEventListener('timeupdate', () => {
        if (!audio.duration) return;
        const f = audio.currentTime / audio.duration;
        el.seek.value = Math.round(f * 1000);
        setRange(el.seek, f);
        el.timeNow.textContent = fmt(audio.currentTime);
    });

    audio.addEventListener('loadedmetadata', () => {
        el.timeTotal.textContent = fmt(audio.duration);
    });

    audio.addEventListener('error', () => {
        if (currentTrack()) el.nowArtist.textContent = 'Файл не найден или не воспроизводится';
        syncPlayState();
    });

    function syncPlayState() {
        const isPlaying = !audio.paused && !audio.ended;
        el.player.classList.toggle('is-playing', isPlaying);
        el.btnPlay.setAttribute('aria-label', isPlaying ? 'Пауза' : 'Играть');

        const playing = currentTrack();
        document.querySelectorAll('.row').forEach((row) => {
            const isCurrent = playing && Number(row.dataset.id) === playing.id;
            row.classList.toggle('is-current', Boolean(isCurrent));
            row.classList.toggle('is-playing', Boolean(isCurrent) && isPlaying);
        });
    }

    // ---------- управление ----------
    el.list.addEventListener('click', (e) => {
        const row = e.target.closest('.row');
        if (row && !row.disabled) playFromList(Number(row.dataset.id));
    });

    el.genres.addEventListener('click', (e) => {
        const chip = e.target.closest('.chip');
        if (!chip) return;
        genre = chip.dataset.genre;
        el.genres.querySelectorAll('.chip').forEach((c) => c.classList.toggle('is-active', c === chip));
        applyFilters();
    });

    el.search.addEventListener('input', () => {
        query = el.search.value;
        applyFilters();
    });

    el.btnPlay.addEventListener('click', togglePlay);
    el.btnNext.addEventListener('click', () => step(1));
    el.btnPrev.addEventListener('click', prev);

    el.seek.addEventListener('input', () => {
        if (!audio.duration) return;
        const f = el.seek.value / 1000;
        audio.currentTime = f * audio.duration;
        setRange(el.seek, f);
    });

    el.volume.addEventListener('input', () => {
        const v = el.volume.value / 100;
        audio.volume = v;
        setRange(el.volume, v);
        store('volume', el.volume.value);
    });

    // пробел = пауза, если фокус не в поле ввода
    document.addEventListener('keydown', (e) => {
        if (e.code !== 'Space') return;
        const tag = document.activeElement?.tagName;
        if (tag === 'INPUT' || tag === 'TEXTAREA' || tag === 'BUTTON') return;
        e.preventDefault();
        togglePlay();
    });

    // ---------- старт ----------
    const savedVolume = load('volume');
    if (savedVolume !== null && !Number.isNaN(Number(savedVolume))) el.volume.value = savedVolume;
    audio.volume = el.volume.value / 100;
    setRange(el.volume, el.volume.value / 100);
    setRange(el.seek, 0);

    loadTracks();
})();
