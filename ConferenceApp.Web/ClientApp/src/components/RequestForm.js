import React, { useState } from "react";

import styled from "styled-components";

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

const CustomInputFile = styled.label`
  border: 1px solid #ccc;
  background-color: white;
  display: flex;
  border-radius: 10px 10px 10px 10px;
  width: 100%;
  height: 30px;
  cursor: pointer;
  padding-left: 5px;
  padding-top: 3px;
  color: black;
`;

const InputFile = styled.input`
  display: none;
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
  width: 300px;
`;

const ButtonSendForm = styled.button`
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

const fieldAfter = [
  { key: 'surname', id: 'surname', value: 'Фамилия' },
  { key: 'name', id: 'name', value: 'Имя' },
  { key: 'patronymic', id: 'patronymic', value: 'Отчество' },
  { key: 'academic-degree', id: 'academic-degree', value: 'Ученая степень, звание' },
  { key: 'organization', id: 'organization', value: 'Организация' },
  { key: 'mailing-address', id: 'mailing-address', value: 'Почтовый адрес' },
  { key: 'number', id: 'number', value: 'Телефон' },
  { key: 'fax', id: 'fax', value: 'Факс' },
  { key: 'email', id: 'email', value: 'E-mail' }
]

const fieldBefore = [
  { key: 'article-name', id: 'article-name', value: 'Название доклада' },
  { key: 'collaborators', id: 'collaborators', value: 'Ф.И.О. соавторов' }
]



const RequestForm = () => {

  const [fileName, setFileName] = useState(null);

  return (<Section id="participant-form">
    <Form>
      <FormGroup>
        <Title>Информационная карта участника (заявка)</Title>
        {fieldAfter.map(f => <InputText placeholder={f.value} id={f.id} key={f.key} />)}
        <LabelInput>Тип доклада и форма участия</LabelInput>
        <InputSelect>
          <option value="пленарный">пленарный</option>
          <option value="секционный">секционный</option>
          <option value="стендовый">стендовый</option>
          <option value="опубликование в сборнике">опубликование в сборнике</option>
        </InputSelect>
        {fieldBefore.map(f => <InputText placeholder={f.value} id={f.id} key={f.key} />)}
        <CustomInputFile htmlFor="file-upload">{fileName ? fileName : 'Файл доклада'}</CustomInputFile>
        <InputFile id="file-upload" type="file" onChange={(e) => setFileName(e.target.files[0].name)} />
        <LabelInput>Необходимость в гостинице:</LabelInput>
        <Small>Дата приезда</Small>
        <input type="date" id="start" name="date-start" min="2020-01-01" max="2020-12-31"></input>
        <Small>Дата отъезда</Small>
        <input type="date" id="end" name="date-end" min="2020-01-01" max="2020-12-31"></input>
        <ButtonWrap>
          <ButtonSendForm type="submit" onClick={(e) => { e.preventDefault() }}>Отправить</ButtonSendForm>
        </ButtonWrap>
      </FormGroup>
    </Form>
  </Section>);
};

export default RequestForm;