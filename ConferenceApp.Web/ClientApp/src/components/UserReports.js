import React from 'react';
import styled from 'styled-components';

const Table = styled.table`
  border-collapse: collapse;
  width: 95%;
  margin-bottom: 20px;
  margin-left: auto;
  margin-right: auto;
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

const status = ['Отсутствует', 'Утверждено', 'Отклонено'];

const reportType = [
  'Пленарный',
  'Секционный',
  'Стендовый',
  'Опубликование в сборнике'
];

const UserReports = props => {
  return (
    <div>
      {props.reports.length > 0 ? (
        <Table>
          <thead>
            <TableRow>
              <TableHeader>Название</TableHeader>
              <TableHeader>Тип</TableHeader>
              <TableHeader>Соавторы</TableHeader>
              <TableHeader>Статус</TableHeader>
            </TableRow>
          </thead>
          <tbody>
            {props.reports.map(r => (
              <TableRow key={r.id}>
                <TableData>{r.title}</TableData>
                <TableData>{reportType[r.reportType]}</TableData>
                <TableData>{r.collaborators.join(' ')}</TableData>
                <TableData>{status[r.reportStatus]}</TableData>
              </TableRow>
            ))}
          </tbody>
        </Table>
      ) : (
        <InfoText>Вы пока не добавили ни одного доклада</InfoText>
      )}
    </div>
  );
};

export default UserReports;
