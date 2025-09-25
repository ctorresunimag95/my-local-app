import config from '../../auth_config.json';

export const environment = {
    production: false,
    auth: {
        domain: config.domain,
        clientId: config.clientId,
        authorizationParams: {
            redirect_uri: window.location.origin
        }
    },
    apiUrls: {
        cinema: 'gateway/api'
    }
}