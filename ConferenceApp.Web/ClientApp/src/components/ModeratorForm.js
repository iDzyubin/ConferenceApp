import React, { useState, useEffect } from 'react';
import styled from 'styled-components';

import { tokensStorage } from '../services/tokensStorage';
import { GetAllReports } from '../services/api';

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

const ModeratorForm = () => {
  const [Reports, setReports] = useState([]);
  const [view, setView] = useState(false);
  const [currentReport, setCurrentRequest] = useState(undefined);

  useEffect(() => {
    refresh();
  }, []);

  const degree = [
    'Бакалавр',
    'Магистр',
    'Специалист',
    'Кандидат наук',
    'Доктор наук'
  ];

  const status = ['Отсутствует', 'Утверждено', 'Отклонено'];

  const reportType = [
    'Пленарный',
    'Секционный',
    'Стендовый',
    'Опубликование в сборнике'
  ];

  const getModalState = () => {
    return { display: view ? 'block' : 'none' };
  };

  const refresh = () => {
    GetAllReports(tokensStorage.get().accessToken)
      .catch(error => console.log(error))
      .then(data => {
        setReports(data);
      });
  };

  const approve = id => {
    console.log('rapprove: request id = ', id);
  };

  const reject = id => {
    console.log('reject: request id = ', id);
  };

  const openModalWindow = request => {
    setCurrentRequest(request);
    setView(!view);
  };

  return (
    <div>
      <ButtonWrap>
        <Button type='button' onClick={refresh}>
          Обновить список заявок
        </Button>
      </ButtonWrap>
      {Reports.length > 0 ? (
        <Table>
          <thead>
            <TableRow>
              <TableHeader>Название</TableHeader>
              <TableHeader>Тип</TableHeader>
              <TableHeader>Соавторы</TableHeader>
              <TableHeader>Файл доклада</TableHeader>
              <TableHeader>Действия</TableHeader>
              <TableHeader>Статус</TableHeader>
            </TableRow>
          </thead>
          <tbody>
            {Reports.map(r => (
              <TableRow key={r.id}>
                <TableData>{r.Title}</TableData>
                <TableData>{reportType[r.ReportType]}</TableData>
                <TableData>{r.Collaborators.join(' ')}</TableData>
                <TableData>{r.File}</TableData>
                <TableData>
                  <MiniButton
                    style={{ backgroundColor: 'green' }}
                    type='button'
                    onClick={() => approve(r.id)}>
                    Одобрить
                  </MiniButton>
                  <MiniButton
                    style={{ backgroundColor: 'red' }}
                    type='button'
                    onClick={() => reject(r.id)}>
                    Отклонить
                  </MiniButton>
                </TableData>
                <TableData>{status[r.ReportStatus]}</TableData>
              </TableRow>
            ))}
          </tbody>
        </Table>
      ) : (
        <InfoText>Докладов нет.</InfoText>
      )}
      {currentReport ? (
        <ModalWindow style={getModalState()}>
          <ModalContent>
            <ModalCloseButton onClick={() => setView(!view)}>
              &times;
            </ModalCloseButton>
          </ModalContent>
        </ModalWindow>
      ) : null}
    </div>
  );
};

export default ModeratorForm;
