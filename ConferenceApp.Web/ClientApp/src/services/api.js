import { localStorage } from './localStorage';
import { download } from '../services/download';
import { navigate } from 'hookrouter';

const RefreshToken = async token => {
  const response = await fetch(`/token/${token}/refresh`, {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    }
  });
  try {
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};

export const checkRespone = resp => {
  return (
    resp.hasOwnProperty('jsonWebToken') &&
    resp.hasOwnProperty('role') &&
    resp.hasOwnProperty('userId') &&
    resp.hasOwnProperty('fullName')
  );
};

const checkTokens = resp => {
  return (
    resp.hasOwnProperty('accessToken') &&
    resp.hasOwnProperty('refreshToken') &&
    resp.hasOwnProperty('expires')
  );
};

export const checkToken = () => {
  const date = new Date();
  const timestamp = Math.floor(
    (date.getTime() - date.getTimezoneOffset() * 60 * 1000) / 1000
  );
  const localData = localStorage.get();
  if (
    localData &&
    checkRespone(localData) &&
    localData.jsonWebToken.expires < timestamp
  ) {
    RefreshToken(localData.jsonWebToken.refreshToken)
      .catch(() => {
        localStorage.remove();
        navigate('/signin');
      })
      .then(data => {
        if (checkTokens(data)) {
          localStorage.add({ ...localData, jsonWebToken: data });
        } else {
          localStorage.remove();
          navigate('/signin');
        }
      });
  }
};

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
};

export const Logout = async token => {
  checkToken();
  const response = await fetch('/token/cancel', {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
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
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
};

export const UpdateReport = async (report, file, token, reportId) => {
  checkToken();
  const response = await fetch(`/api/report/${reportId}`, {
    method: 'PUT',
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

export const SendReport = async (report, file, token) => {
  checkToken();
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
  formData.append('file', file);
  const response = await fetch(`/api/report/${id}/upload`, {
    method: 'POST',
    mode: 'cors',
    headers: {
      Accept: 'multipart/form-data',
      Authorization: `Bearer ${token}`
    },
    body: formData
  });
  try {
    const res = await response.status;
    if (file) {
      return res === 200;
    } else {
      return res === 204;
    }
  } catch (e) {
    throw new Error(e);
  }
};

export const GetReportsByUser = async (id, token) => {
  checkToken();
  const response = await fetch(`/api/report/get-reports-by-user/${id}`, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const data = await response.json();
    return data;
  } catch (e) {
    throw new Error(e);
  }
};

export const GetAllReports = async token => {
  checkToken();
  const response = await fetch('/api/report/', {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    if (res !== 200) {
      return false;
    }
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};

export const GetUser = async (id, token) => {
  checkToken();
  const response = await fetch(`api/user/${id}`, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    if (res !== 200) {
      return false;
    }
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};

export const UpdateUser = async (id, token, user) => {
  checkToken();
  const response = await fetch(`api/user/${id}`, {
    method: 'PUT',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(user)
  });
  try {
    const res = await response.status;
    if (res !== 200) {
      return false;
    }
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};

export const FindUser = async (token, email) => {
  checkToken();
  const response = await fetch(`api/user/${email}/is-exists`, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
};

export const GetAllUsers = async token => {
  checkToken();
  const response = await fetch('api/user/confirmed', {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    if (res !== 200) {
      return false;
    }
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};

export const DeleteReport = async (id, token) => {
  checkToken();
  const response = await fetch(`/api/report/${id}/detach`, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    return res === 204;
  } catch (e) {
    return e;
  }
};

export const ApproveReport = async (id, token) => {
  checkToken();
  const response = await fetch(`/api/report/${id}/approve`, {
    method: 'PUT',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
};

export const RejectReport = async (id, token) => {
  checkToken();
  const response = await fetch(`/api/report/${id}/reject`, {
    method: 'PUT',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
};

export const DownloadReport = async (id, token, name, type) => {
  checkToken();
  return fetch(`/api/report/${id}/download`, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  })
    .then(function(resp) {
      return resp.blob();
    })
    .then(function(blob) {
      download(blob, `${name}${type}`, blob.type);
    });
};

export const GetAllSection = async token => {
  checkToken();
  const response = await fetch('api/section', {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    if (res !== 200) {
      return false;
    }
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};

export const CreateSection = async (token, section) => {
  checkToken();
  const response = await fetch('api/section', {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(section)
  });
  try {
    const res = await response.status;
    if (res !== 200) {
      return false;
    }
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};

export const UpdateSection = async (token, section) => {
  checkToken();
  const response = await fetch(`api/section/${section.id}`, {
    method: 'PUT',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    },
    body: JSON.stringify(section)
  });
  try {
    const res = await response.status;
    return res === 204;
  } catch (e) {
    return e;
  }
};

export const DeleteSection = async (token, id) => {
  checkToken();
  const response = await fetch(`api/section/${id}`, {
    method: 'DELETE',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
      Authorization: `Bearer ${token}`
    }
  });
  try {
    const res = await response.status;
    return res === 204;
  } catch (e) {
    return e;
  }
};

export const SetSectionToReport = async (token, reportId, sectionId) => {
  checkToken();
  const response = await fetch(
    `api/section/${reportId}/attach-to/${sectionId}`,
    {
      method: 'POST',
      mode: 'cors',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: `Bearer ${token}`
      }
    }
  );
  try {
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
};

export const UnsetSectionToReport = async (token, reportId, sectionId) => {
  checkToken();
  const response = await fetch(
    `api/section/${reportId}/detach-from/${sectionId}`,
    {
      method: 'POST',
      mode: 'cors',
      headers: {
        'Content-Type': 'application/json',
        Accept: 'application/json',
        Authorization: `Bearer ${token}`
      }
    }
  );
  try {
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
};

export const UploadRndFile = async (token, file) => {
  checkToken();
  const formData = new FormData();
  formData.append('file', file);
  const response = await fetch(`/api/compilation/upload`, {
    method: 'POST',
    mode: 'cors',
    headers: {
      Accept: 'multipart/form-data',
      Authorization: `Bearer ${token}`
    },
    body: formData
  });
  try {
    const res = await response.status;
    return res === 200;
  } catch (e) {
    return e;
  }
};

export const DownloadRndFile = async (compilationId, name) => {
  return fetch(`/api/compliation/download/${compilationId}`, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    }
  })
    .then(function(resp) {
      return resp.blob();
    })
    .then(function(blob) {
      download(blob, `${name}`, blob.type);
    });
};

export const GetAllFiles = async () => {
  const response = await fetch(`/api/compilation`, {
    method: 'GET',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json'
    }
  });
  try {
    const res = await response.status;
    if (res !== 200) {
      return false;
    }
    const data = await response.json();
    return data;
  } catch (e) {
    return e;
  }
};
