const responseBox = document.getElementById('response-box');
const responseStatus = document.getElementById('response-status');
const responseTime = document.getElementById('response-time');
const responseBody = document.getElementById('response-body');

async function callApi(method, url, body) {
    const start = performance.now();
    responseBox.classList.remove('is-hidden');
    responseBox.classList.add('is-loading');
    responseStatus.textContent = `${method} ${url} …`;
    responseStatus.className = 'response-status';
    responseTime.textContent = '';
    responseBody.textContent = 'Загрузка…';

    const options = {
        method,
        headers: { Accept: 'application/json' }
    };

    if (body !== undefined) {
        options.headers['Content-Type'] = 'application/json';
        options.body = JSON.stringify(body);
    }

    try {
        const res = await fetch(url, options);
        const elapsed = Math.round(performance.now() - start);
        const text = await res.text();

        responseStatus.textContent = `${method} ${url} — ${res.status}`;
        responseStatus.classList.add(res.ok ? 'is-ok' : 'is-error');
        responseTime.textContent = `${elapsed} ms`;

        try {
            responseBody.textContent = JSON.stringify(JSON.parse(text), null, 2);
        } catch {
            responseBody.textContent = text || '(пустой ответ)';
        }
    } catch (err) {
        responseStatus.textContent = `${method} ${url} — ошибка`;
        responseStatus.classList.add('is-error');
        responseTime.textContent = '';
        responseBody.textContent = err.message;
    } finally {
        responseBox.classList.remove('is-loading');
    }
}

function val(id) {
    return document.getElementById(id).value.trim();
}

function num(id) {
    return Number(document.getElementById(id).value);
}

const actions = {
    'get-tracks': () => callApi('GET', '/tracks'),

    'get-track': () => callApi('GET', `/tracks/${num('get-track-id')}`),

    'post-track': () => callApi('POST', '/tracks', {
        title: val('post-track-title'),
        artistId: num('post-track-artist'),
        genreId: num('post-track-genre'),
        durationSeconds: num('post-track-duration')
    }),

    'put-track': () => callApi('PUT', `/tracks/${num('put-track-id')}`, {
        title: val('put-track-title'),
        artistId: num('put-track-artist'),
        genreId: num('put-track-genre'),
        durationSeconds: num('put-track-duration')
    }),

    'delete-track': () => callApi('DELETE', `/tracks/${num('delete-track-id')}`),

    'post-artist': () => callApi('POST', '/artists', {
        name: val('post-artist-name')
    }),

    'post-genre': () => callApi('POST', '/Genres', {
        name: val('post-genre-name')
    })
};

document.querySelectorAll('[data-action]').forEach(btn => {
    btn.addEventListener('click', () => {
        const handler = actions[btn.dataset.action];
        if (handler) handler();
    });
});
