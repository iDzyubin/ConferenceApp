export const localStorage = {
  data: undefined,

  get: () => {
    localStorage.getFromLocalStorage();
    return localStorage.data;
  },

  add: data => {
    localStorage.data = data;
    localStorage.saveToLocalStorage();
  },

  remove: () => {
    localStorage.data = undefined;
    localStorage.saveToLocalStorage();
  },

  saveToLocalStorage: () => {
    const jsonText = JSON.stringify(localStorage.data);
    window.localStorage.setItem('data', jsonText);
  },

  getFromLocalStorage: () => {
    const jsonText = window.localStorage.getItem('data');
    if (jsonText !== 'undefined') {
      localStorage.data = JSON.parse(jsonText);
    }
  }
};
