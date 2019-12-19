import React, { useState } from 'react';
import styled from 'styled-components';

import * as Api from '../services/api';

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

const LabelInput = styled.label`
  margin-left: auto;
  margin-right: auto;
  color: #1d4dbb;
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

const MiniButton = styled.button`
  font-family: 'Montserrat', sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  background-color: #5172bf;
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

const InputSelect = styled.select`
  display: flex;
  justify-content: space-around;
  border-radius: 10px 10px 10px 10px;
  width: 95%;
  padding-left: 5px;
  height: 30px;
  font-size: 15px;
  margin-left: 20px;
  margin-right: 20px;
  margin-bottom: 20px;
`;

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 10px;
  color: red;
`;

const InputText = styled.input`
  display: flex;
  border-radius: 10px 10px 10px 10px;
  justify-content: space-around;
  width: 95%;
  color: black;
  margin-left: 20px;
  margin-right: 20px;
  padding-left: 20px;
  margin-bottom: 20px;
`;

const InputFile = styled.input`
  border-radius: 10px 10px 10px 10px;
  border: 2px dotted black;
  width: 95%;
  background-color: grey;
`;

const InputFileWrap = styled.div`
  min-width: 400px;
`;

const Form = styled.form``;
const FormGroup = styled.div``;

const ReportForm = props => {
  const [view, setView] = useState(false);

  const [Title, setTitle] = useState('');
  const [ReportType, setReportType] = useState(0);
  const [File, setFile] = useState(null);
  const [Collaborators, setCollaborators] = useState([]);

  const [error, setError] = useState(null);

  const addCollaborator = () => {
    setCollaborators([...Collaborators, '']);
  };

  const editCollaborator = (e, i) => {
    const newCollaborators = Collaborators.slice();
    setCollaborators(
      newCollaborators.map((c, index) => {
        if (index === i) {
          return e.target.value;
        } else {
          return c;
        }
      })
    );
  };

  const prepareData = (f, r) => {
    // const formData = new FormData();
    // const res = {};
    // f.filter(elem => elem.value !== '').forEach(elem => {
    //   res[elem.key] = elem.value;
    // });
    // res['degree'] = academicDegree;
    // const reports = [];
    // const guids = [];
    // r.forEach(elem => {
    //   const newId = uuid();
    //   const newReport = {};
    //   newReport.title = elem.title;
    //   newReport.reportType = elem.reportType;
    //   newReport.Collaborators = elem.Collaborators;
    //   newReport.file = newId;
    //   guids.push(newId);
    //   reports.push(newReport);
    // });
    // formData.append('request', JSON.stringify({ user: { ...res }, reports }));
    // let i = 0;
    // r.forEach(elem => {
    //   formData.append(guids[i], elem.file);
    // });
    // return formData;
  };

  const handleSubmit = async e => {
    let formObj = document.getElementById('report-form');
    if (formObj.checkValidity()) {
      const fColl = Collaborators.filter(c => !!c);
      const report = {
        userid: props.userId,
        title: Title,
        reportType: ReportType,
        collaborators: fColl
      };
      Api.SendReport(report, File, props.token)
        .catch(() => setError(true))
        .then(response => {
          console.log(response);

          // if (checkRespone(response)) {
          //   setAuthError(null);
          //   navigate('/signin');
          // } else {
          //   setAuthError('Ошибка регистрации');
          // }
        });
    }
  };

  const openModalWindow = () => {
    setView(!view);
  };

  const getModalState = () => {
    return { display: view ? 'block' : 'none' };
  };

  const handleSelectInput = event => {
    setReportType(parseInt(event.target.value));
  };

  return (
    <div>
      <Button type='button' onClick={() => openModalWindow()}>
        Добавить доклад
      </Button>
      <ModalWindow style={getModalState()}>
        <ModalContent>
          <ModalCloseButton onClick={() => setView(!view)}>
            &times;
          </ModalCloseButton>
          <Form id='report-form' onSubmit={e => e.preventDefault()}>
            <FormGroup>
              <InputText
                placeholder='Название доклада'
                onChange={e => setTitle(e.target.value)}
                required
              />
              <InputSelect
                id='reportType'
                onChange={handleSelectInput}
                value={ReportType}>
                <option value='0'>Пленарный</option>
                <option value='1'>Секционный</option>
                <option value='2'>Стендовый</option>
              </InputSelect>
              <LabelInput>
                Соавторы добавляются по e-mail, который использовался при
                регистрации
              </LabelInput>
              <br />
              {Collaborators.map((c, i) => (
                <InputText
                  key={i}
                  placeholder={`Соавтор №${i + 1}`}
                  onChange={e => editCollaborator(e, i)}></InputText>
              ))}
              <MiniButton type='button' onClick={addCollaborator}>
                Добавить соавтора
              </MiniButton>
              <InputFileWrap>
                <InputFile
                  id='file'
                  type='file'
                  onChange={e => setFile(e.target.files[0])}
                />
              </InputFileWrap>
              <ButtonWrap>
                <Button type='submit' onClick={handleSubmit}>
                  Добавить доклад
                </Button>
              </ButtonWrap>
              {error && <InfoText>{error}</InfoText>}
            </FormGroup>
          </Form>
        </ModalContent>
      </ModalWindow>
    </div>
  );
};

export default ReportForm;
