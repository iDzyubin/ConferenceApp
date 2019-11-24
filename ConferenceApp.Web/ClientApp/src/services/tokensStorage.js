export const tokenStorage = {
  token: undefined,

  get: () => {
    tokenStorage.getFromLocalStorage();
    return tokenStorage.token;
  },

  add: token => {
    tokenStorage.token = token;
    tokenStorage.saveToLocalStorage();
  },

  remove: () => {
    tokenStorage.token = undefined;
    tokenStorage.saveToLocalStorage();
  },

  saveToLocalStorage: () => {
    const jsonText = JSON.stringify(tokenStorage.token);
    window.localStorage.setItem('token', jsonText);
  },

  getFromLocalStorage: () => {
    const jsonText = window.localStorage.getItem('token');
    if (jsonText !== 'undefined') {
      tokenStorage.token = JSON.parse(jsonText);
    }
  }
};

export const refreshTokenStorage = {
  refreshToken: undefined,

  get: () => {
    refreshTokenStorage.getFromLocalStorage();
    return refreshTokenStorage.refreshToken;
  },

  add: refreshToken => {
    refreshTokenStorage.token = refreshToken;
    refreshTokenStorage.saveToLocalStorage();
  },

  remove: () => {
    refreshTokenStorage.refreshToken = undefined;
    refreshTokenStorage.saveToLocalStorage();
  },

  saveToLocalStorage: () => {
    const jsonText = JSON.stringify(refreshTokenStorage.refreshToken);
    window.localStorage.setItem('refreshToken', jsonText);
  },

  getFromLocalStorage: () => {
    const jsonText = window.localStorage.getItem('refreshToken');
    if (jsonText !== 'undefined') {
      refreshTokenStorage.refreshToken = JSON.parse(jsonText);
    }
  }
};
