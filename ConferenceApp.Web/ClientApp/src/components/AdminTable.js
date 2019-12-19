import React, { useState, useEffect } from 'react';
import styled from "styled-components";

import useGlobal from '../store';
import { tokensStorage } from '../services/tokensStorage'
import { GetAllRequests } from '../services/api'

const Card = styled.div`
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  max-width: 99%;
  margin: auto;
  margin-top: 30px;
  text-align: center;
  font-family: arial;
`;

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: space-around;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
  width: 500px;
`;

const Button = styled.button`
  font-family: "Montserrat", sans-serif;
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
  font-family: "Montserrat", sans-serif;
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
  width: 100%;
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

const TableRow = styled.tr`
  :hover {
    background-color: #cccccc;
  }
`;

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  font-size: 20px;
  color: #5172bf;
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
  background-color: rgb(0,0,0);
  background-color: rgba(0,0,0,0.4);
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

const AdminTable = () => {

  const [globalState, globalActions] = useGlobal();
  const [requests, setRequests] = useState([]);
  const [view, setView] = useState(false);
  const [currentRequest, setCurrentRequest] = useState(undefined);

  useEffect(() => {
    refresh();
  }, []);

  const degree = [
    'Бакалавр',
    'Магистр',
    'Специалист',
    'Кандидат наук',
    'Доктор наук'
  ]

  const reportType = [
    'Пленарный',
    'Секционный',
    'Стендовый',
    'Опубликование в сборнике'
  ]

  const getModalState = () => {
    return { display: view ? "block" : "none" }
  }

  const refresh = () => {
    GetAllRequests(tokensStorage.get().accessToken)
      .catch(error => console.log(error))
      .then(data => {
        setRequests(data);
      });
  }

  const logOut = () => {
    tokensStorage.remove();
    globalActions.setAuth(false);
  }

  const approveAll = id => {
    console.log('rapproveAll: request id = ', id);
  }

  const rejectAll = id => {
    console.log('rejectAll: request id = ', id);
  }

  const approve = id => {
    console.log('rapprove: request id = ', id);
  }

  const reject = id => {
    console.log('reject: request id = ', id);
  }

  const openModalWindow = request => {
    setCurrentRequest(request);
    setView(!view);
  }

  return (
    <Card>
      <InfoText style={{ fontSize: 25 }}>Добро пожаловать в режим администратора.</InfoText>
      <ButtonWrap>
        <Button type="button" onClick={refresh}>Обновить список заявок</Button>
        <Button style={{ backgroundColor: "red" }} type="button" onClick={logOut}>Выйти</Button>
      </ButtonWrap>
      {requests.length > 0 ?
        <Table>
          <thead>
            <TableRow>
              <TableHeader>Имя</TableHeader>
              <TableHeader>Фамилия</TableHeader>
              <TableHeader>Отчество</TableHeader>
              <TableHeader>Ученая степень, звание</TableHeader>
              <TableHeader>Организация</TableHeader>
              <TableHeader>Почтовый адрес</TableHeader>
              <TableHeader>Телефон</TableHeader>
              <TableHeader>Факс</TableHeader>
              <TableHeader>E-mail</TableHeader>
              <TableHeader>Просмотрено</TableHeader>
            </TableRow>
          </thead>
          <tbody>
            {requests.map(r =>
              <TableRow onClick={() => openModalWindow(r)} key={r.id}>
                <TableData>{r.firstName}</TableData>
                <TableData>{r.lastName}</TableData>
                <TableData>{r.middleName}</TableData>
                <TableData>{degree[r.degree]}</TableData>
                <TableData>{r.organization}</TableData>
                <TableData>{r.address}</TableData>
                <TableData>{r.phone}</TableData>
                <TableData>{r.fax}</TableData>
                <TableData>{r.email}</TableData>
                <TableData>0/1</TableData>
              </TableRow>
            )}
          </tbody>
        </Table> : <InfoText>Заявок нет.</InfoText>
      }
      {currentRequest ? <ModalWindow style={getModalState()}>
        <ModalContent>
          <Button style={{ backgroundColor: "green" }} type="button" onClick={() => approveAll(currentRequest.id)}>Одобрить все</Button>
          <Button style={{ backgroundColor: "red" }} type="button" onClick={() => rejectAll(currentRequest.id)}>Отклонить все</Button>
          <ModalCloseButton onClick={() => setView(!view)}>&times;</ModalCloseButton>
          <Table>
            <thead>
              <TableRow>
                <TableHeader>Название доклада</TableHeader>
                <TableHeader>Тип доклада и форма участия</TableHeader>
                <TableHeader>Ф.И.О. соавторов</TableHeader>
                <TableHeader>Файл доклада</TableHeader>
                <TableHeader>Статус</TableHeader>
                <TableHeader>Действия</TableHeader>
              </TableRow>
            </thead>
            <tbody>
              {
                currentRequest.reports.map(r => {
                  return (<TableRow key={r.id}>
                    <TableData>{r.title}</TableData>
                    <TableData>{reportType[r.reportType]}</TableData>
                    <TableData>{r.Collaborators}</TableData>
                    <TableData>{r.file}</TableData>
                    <TableData>{r.status}</TableData>
                    <TableData>
                      <MiniButton style={{ backgroundColor: "green" }} type="button" onClick={() => approve(r.id)}>Одобрить</MiniButton>
                      <MiniButton style={{ backgroundColor: "red" }} type="button" onClick={() => reject(r.id)}>Отклонить</MiniButton>
                    </TableData>
                  </TableRow>)
                })}
            </tbody>
          </Table>
        </ModalContent>
      </ModalWindow> : null}
    </Card>
  );
};

export default AdminTable;