export const SignIn = async user => {
  const response = await fetch('/api/account/signin', {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    },
    body: JSON.stringify(user)
  });
  try {
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
  // return {
  //   accessToken: 'asd',
  //   expires: 123123,
  //   refreshToken: 'qwe',
  //   role: 0,
  //   id: '1'
  // };
};

export const SignUp = async user => {
  const response = await fetch('/api/account/signup', {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    },
    body: JSON.stringify(user)
  });
  try {
    const ReportStatus = await response.ReportStatus;
    const data = await response.json();
    return { ReportStatus, data };
  } catch (e) {
    return e;
  }
};

export const SendReport = async (report, file, token) => {
  const response = await fetch(`/api/report/attach-to/${report.userid}`, {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(report)
  });
  try {
    const data = await response.json();
    return UploadFile(file, token, data.value.id);
  } catch (e) {
    return e;
  }
};

export const UploadFile = async (file, token, id) => {
  const formData = new FormData();
  formData.append(id, file);
  const response = await fetch(`/api/report/${id}/upload`, {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'multipart/form-data',
      Accept: 'multipart/form-data',
      Authorization: `Bearer ${token}`
    },
    body: formData
  });
  try {
    const data = await response.json();
    return data;
  } catch (e) {
    throw new Error(e);
  }
};

export const GetUserRole = async id => {
  const response = await fetch('/api/request/Create', {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    },
    body: id
  });
  try {
    const data = await response.json();
    return data;
  } catch (e) {
    throw new Error(e);
  }
};

export const GetAllReports = async token => {
  // const response = await fetch('/api/request/All', {
  //   method: 'GET',
  //   mode: 'cors',
  //   headers: {
  //     'Content-Type': 'application/json',
  //     Accept: 'application/json',
  //     token
  //   }
  // });
  // try {
  //   const data = await response.json();
  //   return data;
  // } catch (e) {
  //   return e;
  // }
  return [
    {
      id: '1',
      Title: 'Исследование название',
      ReportType: 0,
      File: 'Исследование ',
      Collaborators: ['Соавтор 1', 'Соавтор 2', 'Соавтор 3'],
      ReportStatus: 0
    },
    {
      id: '2',
      Title: 'Исследование название1',
      ReportType: 0,
      File: 'Исследование 1',
      Collaborators: [],
      ReportStatus: 0
    },
    {
      id: '3',
      Title: 'Исследование название2',
      ReportType: 1,
      File: 'Исследование 2',
      Collaborators: ['Соавтор 1'],
      ReportStatus: 1
    }
  ];
};
