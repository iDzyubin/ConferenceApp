export const tokensStorage = {
  tokens: undefined,

  get: () => {
    tokensStorage.getFromLocalStorage();
    return tokensStorage.tokens;
  },

  add: tokens => {
    tokensStorage.tokens = tokens;
    tokensStorage.saveToLocalStorage();
  },

  remove: () => {
    tokensStorage.tokens = undefined;
    tokensStorage.saveToLocalStorage();
  },

  saveToLocalStorage: () => {
    const jsonText = JSON.stringify(tokensStorage.tokens);
    window.localStorage.setItem('tokens', jsonText);
  },

  getFromLocalStorage: () => {
    const jsonText = window.localStorage.getItem('tokens');
    if (jsonText !== 'undefined') {
      tokensStorage.tokens = JSON.parse(jsonText);
    }
  }
};
