export const SignIn = async user => {
  const response = await fetch("/api/account/signin", {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    body: JSON.stringify(user),
  });
  try {
    const data = await response.json();
    return data;
  }
  catch (e) {
    return e;
  }
};

export const SendRequest = async request => {
  const response = await fetch("/api/request/Create", {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'multipart/form-data',
      Accept: 'multipart/form-data',
    },
    body: request,
  });
  try {
    const data = await response.json();
    return data;
  }
  catch (e) {
    throw new Error(e);
  }
};

export const GetAllRequests = async token => {
  const response = await fetch("/api/request/All", {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      token
    }
  });
  try {
    const data = await response.json();
    return data;
  }
  catch (e) {
    return e;
  }
  // return [
  //   {
  //     id: "1",
  //     firstName: "Петя",
  //     middleName: "Иванович",
  //     lastName: "Васечкин",
  //     degree: 0,
  //     organization: "ЮЗГУ",
  //     address: "ул. Пушкина, дом Колотушкина",
  //     phone: "88005553535",
  //     fax: "88005553535",
  //     email: "petya@swsu.ru",
  //     reports: [
  //       {
  //         id: "1",
  //         "title": "Исследование зеленого слоника",
  //         "reportType": 0,
  //         "file": "Исследование зеленого слоника",
  //         "Collaborators": "какие то там хуилы которых много и мы будем имитировать длинную строку из дохуя авторов",
  //         "status": 0
  //       }
  //     ]
  //   },
  //   {
  //     id: "2",
  //     firstName: "Петя1",
  //     middleName: "Иванович1",
  //     lastName: "Васечкин1",
  //     degree: 1,
  //     organization: "ЮЗГУ1",
  //     address: "ул. Пушкина, дом Колотушкина1",
  //     phone: "18005553535",
  //     fax: "18005553535",
  //     email: "petya@swsu.ru1",
  //     reports: [
  //       {
  //         id: "2",
  //         "title": "Исследование зеленого слоника1",
  //         "reportType": 0,
  //         "file": "Исследование зеленого слоника1",
  //         "Collaborators": "",
  //         "status": 0
  //       },
  //       {
  //         id: "3",
  //         "title": "Исследование зеленого слоника2",
  //         "reportType": 1,
  //         "file": "Исследование зеленого слоника2",
  //         "Collaborators": "какие то там хуилы которых немного",
  //         "status": 0
  //       }
  //     ]
  //   },
  // ];
};