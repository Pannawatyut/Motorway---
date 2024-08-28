function onSignIn(googleUser) {
    var id_token = googleUser.getAuthResponse().id_token;
    SendMessage('GameManager', 'OnGoogleSignInSuccess', id_token);
}

function renderGoogleSignInButton() {
    gapi.signin2.render('my-signin2', {
        'scope': 'profile email',
        'width': 240,
        'height': 50,
        'longtitle': true,
        'theme': 'dark',
        'onsuccess': onSignIn
    });
}

function initGoogleSignIn(clientId) {
    gapi.load('auth2', function () {
        gapi.auth2.init({
            client_id: clientId
        }).then(function () {
            renderGoogleSignInButton();
        });
    });
}
