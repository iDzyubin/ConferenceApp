import React, { useState, useEffect } from 'react';
import styled from 'styled-components';
import { Sections as SectionsComponent } from './Sections';
import * as Api from '../services/api';

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: center;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
`;

const Button = styled.button`
  font-family: 'Montserrat', sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: #5172bf;
  border: none;
  margin-bottom: 20px;
  margin-right: 5px;
  padding: 11px 40px;
  transition: opacity 0.5s;

  :hover {
    opacity: 0.5;
    cursor: pointer;
  }
`;

const MiniButton = styled.button`
  font-family: 'Montserrat', sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  border: none;
  margin-right: 5px;
  margin-bottom: 5px;
  transition: opacity 0.5s;

  :hover {
    opacity: 0.5;
    cursor: pointer;
  }
`;

const Table = styled.table`
  border-collapse: collapse;
  width: 95%;
  margin-left: auto;
  margin-right: auto;
`;

const Section = styled.div`
  padding-bottom: 20px;
`;

const TableData = styled.td`
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
`;

const TableHeader = styled.th`
  border: 1px solid #dddddd;
  text-align: left;
  padding: 8px;
`;

const InputSelect = styled.select`
  display: flex;
  justify-content: space-around;
  border-radius: 10px 10px 10px 10px;
  width: 95%;
  height: auto;
  font-size: 15px;
  margin-left: 5px;
  margin-right: 5px;
`;

const TableRow = styled.tr`
  :hover {
    background-color: #cccccc;
  }
`;

const TableWrap = styled.div`
  margin-bottom: 10px;
`;

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  font-size: 20px;
  color: #5172bf;
`;

const ErrorText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  font-size: 20px;
  color: red;
`;

const ModalWindow = styled.div`
  position: fixed;
  z-index: 1;
  padding-top: 100px;
  left: 0;
  top: 0;
  width: 100%;
  height: 100%;
  overflow: auto;
  background-color: rgb(0, 0, 0);
  background-color: rgba(0, 0, 0, 0.4);
`;

const ModalContent = styled.div`
  background-color: #fefefe;
  margin: auto;
  padding: 20px;
  border: 1px solid #888;
  width: 80%;
`;

const ModalCloseButton = styled.span`
  color: #aaaaaa;
  float: right;
  margin-top: -15px;
  font-size: 28px;
  font-weight: bold;

  :hover {
    color: #000;
    text-decoration: none;
    cursor: pointer;
  }

  :focus {
    color: #000;
    text-decoration: none;
    cursor: pointer;
  }
`;

const ModeratorForm = props => {
  const [Reports, setReports] = useState([]);
  const [Users, setUsers] = useState([]);
  const [Sections, setSections] = useState([]);
  const [modal, setModal] = useState(false);
  const [ShowAllUsers, setShowAllUsers] = useState(false);
  const [SectionView, setSectionView] = useState(false);
  const [currentUsers, setCurrentUsers] = useState(undefined);

  const [error, setError] = useState(null);

  useEffect(() => {
    refresh();
  }, []);

  const status = ['Отсутствует', 'Утверждено', 'Отклонено'];

  const reportType = [
    'Пленарный',
    'Секционный',
    'Стендовый',
    'Опубликование в сборнике'
  ];

  const getModalState = state => {
    return { display: state ? 'block' : 'none' };
  };

  const refresh = () => {
    Api.GetAllReports(props.token)
      .catch(() => setError('При получении докладов возникла ошибка'))
      .then(data => {
        if (data) {
          setReports(data);
          setError(null);
        } else {
          setError('При получении докладов возникла ошибка');
        }
      });
    Api.GetAllUsers(props.token)
      .catch(() =>
        setError('При получении списка пользователей возникла ошибка')
      )
      .then(data => {
        if (data) {
          setUsers(data);
          setError(null);
        } else {
          setError('При получении списка пользователей возникла ошибка');
        }
      });
    Api.GetAllSection(props.token)
      .catch(() => setError('При получении списка секций возникла ошибка'))
      .then(data => {
        if (data) {
          setSections(data);
          setError(null);
        } else {
          setError('При получении списка секций возникла ошибка');
        }
      });
  };

  const filterDeleted = id => {
    const newReports = Reports.filter(r => r.id !== id);
    setReports(newReports);
  };

  const setStatus = (id, status) => {
    const newReports = Reports.slice();
    newReports.map(r => {
      if (r.id === id) {
        r.reportStatus = status;
      }
      return r;
    });
    setReports(newReports);
  };

  const setSection = (reportId, sectionId) => {
    const newReports = Reports.slice();
    newReports.map(r => {
      if (r.id === reportId) {
        r.sectionId = sectionId;
      }
      return r;
    });
    setReports(newReports);
  };

  const approve = id => {
    Api.ApproveReport(id, props.token)
      .catch(() => setError('При утверждении доклада возникла ошибка'))
      .then(data => {
        if (data) {
          setStatus(id, 1);
        } else {
          setError('При утверждении доклада возникла ошибка');
        }
      });
  };

  const download = (id, name, fileName) => {
    const type = fileName.match(/\.[0-9a-z]+$/i)[0];
    Api.DownloadReport(id, props.token, name, type);
  };

  const deleteReport = id => {
    Api.DeleteReport(id, props.token)
      .catch(() => setError('При удалении доклада возникла ошибка'))
      .then(data => {
        if (data) {
          filterDeleted(id);
        } else {
          setError('При удалении доклада возникла ошибка');
        }
      });
  };

  const reject = id => {
    Api.RejectReport(id, props.token)
      .catch(() => setError('При отклонении доклада возникла ошибка'))
      .then(data => {
        if (data) {
          setStatus(id, 2);
        } else {
          setError('При отклонении доклада возникла ошибка');
        }
      });
  };

  const handleSelectInputSection = (sectionId, report) => {
    if (sectionId === '00000000-0000-0000-0000-000000000000') {
      Api.UnsetSectionToReport(props.token, report.id, report.sectionId)
        .catch(() => setError('При выборе секции для доклада возникла ошибка'))
        .then(data => {
          if (data) {
            setSection(report.id, sectionId);
          } else {
            setError('При выборе секции для доклада возникла ошибка');
          }
        });
    } else {
      Api.SetSectionToReport(props.token, report.id, sectionId)
        .catch(() => setError('При выборе секции для доклада возникла ошибка'))
        .then(data => {
          if (data) {
            setSection(report.id, sectionId);
          } else {
            setError('При выборе секции для доклада возникла ошибка');
          }
        });
    }
  };

  const toggleModalWindow = report => {
    const author = Users.find(u => u.id === report.userId);
    const collaborators = [];
    Users.forEach(u => {
      report.collaborators.forEach(c => {
        if (c.id === u.id) {
          collaborators.push(u);
        }
      });
    });
    const res = [author, ...collaborators];
    setCurrentUsers(res);
    setModal(!modal);
  };

  const getTitleUsersView = () => {
    return ShowAllUsers ? 'Скрыть' : 'Показать';
  };

  return (
    <Section>
      <ButtonWrap>
        <Button type="button" onClick={refresh}>
          Обновить данные
        </Button>
        <Button type="button" onClick={() => setShowAllUsers(!ShowAllUsers)}>
          {getTitleUsersView()} всех подтвержденных пользователей
        </Button>
        <Button type="button" onClick={() => setSectionView(!SectionView)}>
          Редактирование секций
        </Button>
      </ButtonWrap>

      {ShowAllUsers && (
        <TableWrap>
          {Users.length > 0 ? (
            <Table>
              <thead>
                <TableRow>
                  <TableHeader>E-mail</TableHeader>
                  <TableHeader>Имя</TableHeader>
                  <TableHeader>Отчество</TableHeader>
                  <TableHeader>Фамилия</TableHeader>
                  <TableHeader>Организация</TableHeader>
                  <TableHeader>Телефон</TableHeader>
                  <TableHeader>Адрес организации</TableHeader>
                  <TableHeader>Город организации</TableHeader>
                  <TableHeader>Должность</TableHeader>
                </TableRow>
              </thead>
              <tbody>
                {Users.map(u => (
                  <TableRow key={u.id}>
                    <TableData>{u.email}</TableData>
                    <TableData>{u.firstName}</TableData>
                    <TableData>{u.middleName}</TableData>
                    <TableData>{u.lastName}</TableData>
                    <TableData>{u.organisation}</TableData>
                    <TableData>{u.phone}</TableData>
                    <TableData>{u.organisationAddress}</TableData>
                    <TableData>{u.city}</TableData>
                    <TableData>{u.position}</TableData>
                  </TableRow>
                ))}
              </tbody>
            </Table>
          ) : (
            <InfoText>Подтвержденных пользователей пока нет.</InfoText>
          )}
        </TableWrap>
      )}
      {Reports.length > 0 ? (
        <Table>
          <thead>
            <TableRow>
              <TableHeader>Название</TableHeader>
              <TableHeader>Тип</TableHeader>
              <TableHeader>Соавторы</TableHeader>
              <TableHeader>Файл доклада</TableHeader>
              <TableHeader>Секция</TableHeader>
              <TableHeader>Действия</TableHeader>
              <TableHeader>Статус</TableHeader>
            </TableRow>
          </thead>
          <tbody>
            {Reports.map(r => (
              <TableRow key={r.id}>
                <TableData>{r.title}</TableData>
                <TableData>{reportType[r.reportType]}</TableData>
                <TableData>
                  {r.collaborators.map(c => c.email).join(' ')}
                </TableData>

                <TableData>
                  <MiniButton
                    style={{ backgroundColor: 'steelblue' }}
                    type="button"
                    onClick={() => download(r.id, r.title, r.fileName)}
                  >
                    Скачать
                  </MiniButton>
                </TableData>
                <TableData>
                  <InputSelect
                    onChange={e =>
                      handleSelectInputSection(e.target.value, r)
                    }
                    value={r.sectionId}
                  >
                    <option
                      key="0"
                      value="00000000-0000-0000-0000-000000000000"
                    >
                      Нет секции
                    </option>
                    {Sections.map(s => (
                      <option key={s.id} value={s.id}>
                        {s.title}
                      </option>
                    ))}
                  </InputSelect>
                </TableData>
                <TableData>
                  <MiniButton
                    style={{ backgroundColor: 'green' }}
                    type="button"
                    onClick={() => toggleModalWindow(r)}
                  >
                    Показать авторов
                  </MiniButton>
                  <MiniButton
                    style={{ backgroundColor: 'green' }}
                    type="button"
                    onClick={() => approve(r.id)}
                  >
                    Одобрить
                  </MiniButton>
                  <MiniButton
                    style={{ backgroundColor: 'red' }}
                    type="button"
                    onClick={() => reject(r.id)}
                  >
                    Отклонить
                  </MiniButton>
                  <MiniButton
                    style={{ backgroundColor: 'crimson' }}
                    type="button"
                    onClick={() => deleteReport(r.id)}
                  >
                    Удалить доклад
                  </MiniButton>
                </TableData>
                <TableData>{status[r.reportStatus]}</TableData>
              </TableRow>
            ))}
          </tbody>
        </Table>
      ) : (
        <InfoText>Докладов пока нет.</InfoText>
      )}

      {error && <ErrorText>{error}</ErrorText>}

      {SectionView ? (
        <ModalWindow style={getModalState(SectionView)}>
          <ModalContent>
            <ModalCloseButton onClick={() => setSectionView(!SectionView)}>
              &times;
            </ModalCloseButton>
            <SectionsComponent
              sections={Sections}
              setSections={setSections}
              token={props.token}
            />
          </ModalContent>
        </ModalWindow>
      ) : null}

      {modal ? (
        <ModalWindow style={getModalState(modal)}>
          <ModalContent>
            <ModalCloseButton onClick={() => setModal(!modal)}>
              &times;
            </ModalCloseButton>
            <Table>
              <thead>
                <TableRow>
                  <TableHeader>E-mail</TableHeader>
                  <TableHeader>Имя</TableHeader>
                  <TableHeader>Отчество</TableHeader>
                  <TableHeader>Фамилия</TableHeader>
                  <TableHeader>Организация</TableHeader>
                  <TableHeader>Телефон</TableHeader>
                  <TableHeader>Адрес организации</TableHeader>
                  <TableHeader>Город организации</TableHeader>
                  <TableHeader>Должность</TableHeader>
                </TableRow>
              </thead>
              <tbody>
                {currentUsers.map(u => (
                  <TableRow key={u.id}>
                    <TableData>{u.email}</TableData>
                    <TableData>{u.firstName}</TableData>
                    <TableData>{u.middleName}</TableData>
                    <TableData>{u.lastName}</TableData>
                    <TableData>{u.organisation}</TableData>
                    <TableData>{u.phone}</TableData>
                    <TableData>{u.organisationAddress}</TableData>
                    <TableData>{u.city}</TableData>
                    <TableData>{u.position}</TableData>
                  </TableRow>
                ))}
              </tbody>
            </Table>
          </ModalContent>
        </ModalWindow>
      ) : null}
    </Section>
  );
};

export default ModeratorForm;
