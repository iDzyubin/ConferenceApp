import React from 'react';
import styled from 'styled-components';

const Title = styled.h2`
  font-weight: 400;
  text-align: center;
  color: #1d4dbb;
  font-size: 20px;
  line-height: 1.55;
  padding-top: 20px;
  margin-bottom: 35px;

  @media (min-width: 992px) {
    font-size: 38px;
    line-height: 1.39;
    max-width: 1100px;
    margin-left: auto;
    margin-right: auto;
    margin-bottom: 30px;
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
                <TableData>{r.Title}</TableData>
                <TableData>{reportType[r.ReportType]}</TableData>
                <TableData>{r.Collaborators.join(' ')}</TableData>
                <TableData>{status[r.ReportStatus]}</TableData>
              </TableRow>
            ))}
          </tbody>
        </Table>
      ) : (
        <InfoText>Докладов нет.</InfoText>
      )}
    </div>
  );
};

export default UserReports;
