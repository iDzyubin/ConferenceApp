import React from 'react';
// import useGlobal from '../store';

import styled from 'styled-components';

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
  /* display: none; */
  border-radius: 10px 10px 10px 10px;
  border: 2px dotted black;
  width: 100%;
  background-color: grey;
`;

const InputFileWrap = styled.div`
  min-width: 400px;
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

const LabelInput = styled.label`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  color: #1d4dbb;
`;

const Form = styled.div`
  padding-bottom: 10px;
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

const ReportForm = props => {
  const handleInputs = event => {
    const newReports = props.reports.map(i => {
      if (i.key === props.num + 1) {
        const newProp = {};
        if (event.target.id === 'file') {
          newProp[event.target.id] = event.target.files[0];
        } else {
          newProp[event.target.id] = event.target.value;
        }
        return { ...i, ...newProp };
      } else {
        return i;
      }
    });
    props.setReports(newReports);
  };

  return (
    <Form>
      <LabelInput>Тип доклада и форма участия</LabelInput>
      <InputSelect
        id='reportType'
        onChange={handleInputs}
        value={props.reports[props.num].reportType}>
        <option value='0'>Пленарный</option>
        <option value='1'>Секционный</option>
        <option value='2'>Стендовый</option>
        <option value='3'>Опубликование в сборнике</option>
      </InputSelect>
      <InputText
        placeholder='Название доклада'
        value={props.reports[props.num].title}
        id='title'
        key='title'
        onChange={handleInputs}
      />
      <InputText
        placeholder='Ф.И.О. соавторов'
        value={props.reports[props.num].Collaborators}
        id='Collaborators'
        key='Collaborators'
        onChange={handleInputs}
      />
      <InputFileWrap>
        <InputFile id='file' type='file' onChange={handleInputs} />
      </InputFileWrap>
    </Form>
  );
};

export default ReportForm;
