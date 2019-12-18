import React, { useState } from 'react';
import styled from 'styled-components';

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

const Form = styled.form``;
const FormGroup = styled.div``;

const EditUser = () => {
  const [view, setView] = useState(false);

  const [surname, setSurname] = useState('');
  const [name, setName] = useState();
  const [patronymic, setPatronymic] = useState('');
  const [academicDegree, setAcademicDegree] = useState(0);
  const [organization, setOrganization] = useState('');
  const [mailingAddress, setMailingAddress] = useState('');
  const [number, setNumber] = useState('');
  const [fax, setFax] = useState('');

  const [error, setError] = useState(null);

  const handleSubmit = async e => {
    let formObj = document.getElementById('edit-user-form');
    if (formObj.checkValidity()) {
      const user = {
        FirstName: name,
        LastName: surname,
        Organization: organization,
        Address: mailingAddress,
        Degree: academicDegree,
        phone: number
      };
      // Api.SignUp(user)
      //       .catch(() => setError(true))
      //       .then(response => {
      //         if (checkRespone(response)) {
      //           setAuthError(null);
      //           navigate('/signin');
      //         } else {
      //           setAuthError('Ошибка регистрации');
      //         }
      //       });
    }
  };
  const fields = [
    {
      key: 'lastName',
      str: 'Фамилия',
      value: surname,
      handler: setSurname,
      required: true,
      type: 'text'
    },
    {
      key: 'firstName',
      str: 'Имя',
      value: name,
      handler: setName,
      required: true,
      type: 'text'
    },
    {
      key: 'middleName',
      str: 'Отчество',
      value: patronymic,
      handler: setPatronymic,
      required: false,
      type: 'text'
    },
    {
      key: 'organization',
      str: 'Организация',
      value: organization,
      handler: setOrganization,
      required: true,
      type: 'text'
    },
    {
      key: 'address',
      str: 'Почтовый адрес',
      value: mailingAddress,
      handler: setMailingAddress,
      required: true,
      type: 'text'
    },
    {
      key: 'phone',
      str: 'Телефон',
      value: number,
      handler: setNumber,
      required: false,
      type: 'number'
    },
    {
      key: 'fax',
      str: 'Факс',
      value: fax,
      handler: setFax,
      required: false,
      type: 'text'
    }
  ];

  const openModalWindow = () => {
    setView(!view);
  };

  const getModalState = () => {
    return { display: view ? 'block' : 'none' };
  };

  const handleTextInput = event => {
    const field = fields.find(f => f.key === event.target.id);
    field.handler(event.target.value);
  };

  const handleSelectInput = event => {
    setAcademicDegree(parseInt(event.target.value));
  };

  return (
    <div>
      <Button type='button' onClick={() => openModalWindow()}>
        Изменить данные о себе
      </Button>
      <ModalWindow style={getModalState()}>
        <ModalContent>
          <ModalCloseButton onClick={() => setView(!view)}>
            &times;
          </ModalCloseButton>
          <Form id='edit-user-form' onSubmit={e => e.preventDefault()}>
            <FormGroup>
              {fields.map(f => (
                <InputText
                  placeholder={f.str}
                  id={f.key}
                  key={f.key}
                  onChange={handleTextInput}
                  required={f.required}
                  type={f.type}
                />
              ))}
              <InputSelect
                id='reportType'
                onChange={handleSelectInput}
                value={academicDegree}>
                <option value='0'>Бакалавр</option>
                <option value='1'>Магистр</option>
                <option value='2'>Специалист</option>
                <option value='3'>Кандидат наук</option>
                <option value='4'>Доктор наук</option>
              </InputSelect>
              <ButtonWrap>
                <Button type='submit' onClick={handleSubmit}>
                  Изменить данные
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

export default EditUser;
