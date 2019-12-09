import React, { useState } from "react";
import styled from "styled-components";
import uuid from 'uuid/v4';

import ReportForm from './ReportForm';
import { SendRequest } from '../services/api'

const Section = styled.section`
  padding-top: 30px;
  padding-bottom: 30px;
  padding-left: 5px;
  padding-right: 5px;
  height: 100%;
  background-color: whitesmoke;
`;

const InputText = styled.input`
  display: flex;
  border-radius: 10px 10px 10px 10px;
  justify-content: space-around;
  width: 100%;
  color: black;
  padding-left: 5px;
  margin-bottom: 20px;
`;

const Title = styled.h2`
  font-weight: 400;
  text-align: center;
  color: #1d4dbb;
  font-size: 20px;
  line-height: 1.55;
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

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: space-around;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
  width: 400px;
`;

const InfoBlockSucces = styled.div`
  font-family: "Montserrat", sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: #5172bf;
  border: none;
  padding: 11px 40px;
  transition: background-color 0.5s;

  :hover {
    background-color: #5172bf90;
    cursor: pointer;
  }
`;

const InfoBlockError = styled.div`
  font-family: "Montserrat", sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: red;
  border: none;
  padding: 11px 40px;
  transition: background-color 0.5s;

  :hover {
    background-color: #5172bf90;
    cursor: pointer;
  }
`;

const Button = styled.button`
  font-family: "Montserrat", sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: #5172bf;
  border: none;
  padding: 11px 40px;
  transition: background-color 0.5s;

  :hover {
    background-color: #5172bf90;
    cursor: pointer;
  }
`;

const Form = styled.form``;
const FormGroup = styled.div``;
const LabelInput = styled.label`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  color: #1d4dbb;
`;
const Small = styled.small`
  margin-right: 5px;
  margin-left: 5px;
  color: black;
`;

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  color: red;
`;

const InputSelect = styled.select`
  display: flex;
  justify-content: space-around;
  border-radius: 10px 10px 10px 10px;
  width: 100%;
  padding-left: 5px;
  height: 30px;
  font-size: 15px;
  margin-bottom: 20px;
`;

const RequestForm = () => {

  const [reports, setReports] = useState([{ key: 1, reportType: '0', title: '', Collaborators: '', file: '' }]);

  const addReport = () => {
    setReports([...reports, { key: reports.length + 1, reportType: '0', title: '', Collaborators: '', file: '' }]);
  }

  const [surname, setSurname] = useState('');
  const [name, setName] = useState();
  const [patronymic, setPatronymic] = useState('');
  const [academicDegree, setAcademicDegree] = useState('0');
  const [organization, setOrganization] = useState('');
  const [mailingAddress, setMailingAddress] = useState('');
  const [number, setNumber] = useState('');
  const [fax, setFax] = useState('');
  const [email, setEmail] = useState('');
  const [dataStart, setdataStart] = useState('');
  const [dataEnd, setdataEnd] = useState('');

  const [sended, setSended] = useState(false);
  const [error, setError] = useState(false);
  const [checkFields, setCheckFields] = useState(false);

  const checkRequiredFields = () => {
    return fields.filter(f => requiredFields.find(rf => rf === f.key)).every(f => f.value !== '');
  }

  const checkRequestFields = () => {
    return reports.every(r => r.title !== '' && r.file !== '');
  }

  const prepareData = (f, r) => {

    const formData = new FormData();

    const res = {};
    f.filter(elem => elem.value !== '').forEach(elem => {
      res[elem.key] = elem.value;
    });
    res['degree'] = academicDegree;
    const reports = [];
    const guids = [];
    r.forEach(elem => {
      const newId = uuid();
      const newReport = {};
      newReport.title = elem.title;
      newReport.reportType = elem.reportType;
      newReport.Collaborators = elem.Collaborators;
      newReport.file = newId;
      guids.push(newId);
      reports.push(newReport);
    });
    formData.append('request', JSON.stringify({ user: { ...res }, reports }));
    let i = 0;
    f.forEach(elem => {
      formData.append(guids[i], elem.file);

    })
    return formData;
  }

  const handleSubmit = () => {
    if (checkRequestFields() && checkRequiredFields()) {
      setCheckFields(false);
      SendRequest(prepareData(fields, reports))
        .catch(() => setError(true))
        .then(() => setSended(true));
    } else {
      setCheckFields(true);

    }
  }

  const fields = [
    { key: 'lastName', str: 'Фамилия', value: surname, handler: setSurname },
    { key: 'firstName', str: 'Имя', value: name, handler: setName },
    { key: 'middleName', str: 'Отчество', value: patronymic, handler: setPatronymic },
    { key: 'organization', str: 'Организация', value: organization, handler: setOrganization },
    { key: 'address', str: 'Почтовый адрес', value: mailingAddress, handler: setMailingAddress },
    { key: 'phone', str: 'Телефон', value: number, handler: setNumber },
    { key: 'fax', str: 'Факс', value: fax, handler: setFax },
    { key: 'email', str: 'E-mail', value: email, handler: setEmail }
  ]

  const requiredFields = [
    'surname',
    'name',
    'patronymic',
    'email',
    'number'
  ]

  const handleTextInput = (event) => {
    const field = fields.find(f => f.key === event.target.id);
    field.handler(event.target.value);
  }

  const handleSelectInput = (event) => {
    setAcademicDegree(event.target.value)
  }

  const handleDateStartInput = (event) => {
    setdataStart(event.target.value);
  }

  const handleDataEndInput = (event) => {
    setdataEnd(event.target.value);
  }

  return (<Section id="participant-form">
    <Form>
      <FormGroup>
        <Title>Информационная карта участника (заявка)</Title>
        {fields.map(f => <InputText placeholder={f.str} id={f.key} key={f.key} onChange={handleTextInput} />)}
        <InputSelect id="reportType" onChange={handleSelectInput} value={academicDegree}>
          <option value="0">Бакалавр</option>
          <option value="1">Магистр</option>
          <option value="2">Специалист</option>
          <option value="3">Кандидат наук</option>
          <option value="4">Доктор наук</option>
        </InputSelect>
        {reports.map(f => <ReportForm key={f.key} num={f.key - 1} reports={reports} setReports={setReports} ></ReportForm>)}
        <ButtonWrap>
          <Button type="button" onClick={addReport}>Добавить еще один доклад</Button>
        </ButtonWrap>
        <LabelInput>Необходимость в гостинице:</LabelInput>
        <Small>Дата приезда</Small>
        <input type="date" id="dataStart" min="2020-01-01" max="2020-12-31" onChange={handleDateStartInput} ></input>
        <Small>Дата отъезда</Small>
        <input type="date" id="dataEnd" min="2020-01-01" max="2020-12-31" onChange={handleDataEndInput}></input>
        <ButtonWrap>
          {sended ? error ? <InfoBlockError>Что-то пошло не так. Обратитесь к администратору</InfoBlockError> : <InfoBlockSucces>Заявка успешно отправлена</InfoBlockSucces> : <Button type="button" onClick={handleSubmit} >Отправить</Button>}
        </ButtonWrap>
        {checkFields && <InfoText>Заполните ФИО, номер телефона, e-mail и форму заявки доклада</InfoText>}
      </FormGroup>
    </Form>
  </Section>);
};

export default RequestForm;