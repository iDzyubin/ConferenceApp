import React, { useState } from 'react';
import styled from 'styled-components';

import * as Api from '../services/api';
import { navigate } from 'hookrouter';

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: space-around;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
  width: 500px;
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

const Button = styled.button`
  font-family: 'Montserrat', sans-serif;
  font-size: 15px;
  text-transform: uppercase;
  color: #fff;
  border-radius: 10px 10px 10px 10px;
  background-color: #5172bf;
  border: none;
  margin-bottom: 20px;
  margin-left: 10px;
  margin-right: 10px;
  padding: 11px 40px;
  transition: background-color 0.5s;

  :hover {
    background-color: #5172bf90;
    cursor: pointer;
  }
`;

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

const Line = styled.hr`
  border: 1px solid #f1f1f1;
  margin-bottom: 25px;
`;

const Form = styled.form``;
const FormGroup = styled.div``;

const Card = styled.div`
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  max-width: 800px;
  margin: auto;
  margin-top: 30px;
  text-align: center;
  font-family: arial;
`;

const InfoText = styled.p`
  margin-left: auto;
  margin-right: auto;
  padding-bottom: 10px;
  color: red;
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

const SignUp = () => {
  const [email, setEmail] = useState('');
  const [password1, setPassword1] = useState('');
  const [password2, setPassword2] = useState('');

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
    let formObj = document.getElementById('sign-up-form');
    if (formObj.checkValidity()) {
      if (password1 === password2) {
        const user = {
          Email: email,
          Password: password1,
          FirstName: name,
          LastName: surname,
          Organization: organization,
          Address: mailingAddress,
          Degree: academicDegree,
          phone: number
        };
        Api.SignUp(user)
          .catch(() =>
            setError('Что-то пошло не так. Обратитесь к администратору')
          )
          .then(response => {
            if (checkRespone(response)) {
              setError(null);
              navigate('/signin');
            } else {
              setError('Ошибка регистрации');
            }
          });
      } else {
        setError('Пароли не совпадают');
      }
    }
  };

  const checkRespone = resp => {
    return resp.status === 200;
  };

  const fields = [
    {
      key: 'email',
      str: 'E-mail',
      value: email,
      handler: setEmail,
      required: true,
      type: 'email',
      minlength: 5
    },
    {
      key: 'password1',
      str: 'Пароль (минимум 4 символа)',
      value: password1,
      handler: setPassword1,
      required: true,
      type: 'password',
      minlength: 5
    },
    {
      key: 'password2',
      str: 'Подтверждение пароля',
      value: password2,
      handler: setPassword2,
      required: true,
      type: 'password',
      minlength: 5
    },
    {
      key: 'lastName',
      str: 'Фамилия',
      value: surname,
      handler: setSurname,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'firstName',
      str: 'Имя',
      value: name,
      handler: setName,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'middleName',
      str: 'Отчество',
      value: patronymic,
      handler: setPatronymic,
      required: false,
      type: 'text',
      minlength: 1
    },
    {
      key: 'organization',
      str: 'Организация',
      value: organization,
      handler: setOrganization,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'address',
      str: 'Почтовый адрес',
      value: mailingAddress,
      handler: setMailingAddress,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'phone',
      str: 'Телефон',
      value: number,
      handler: setNumber,
      required: false,
      type: 'number',
      minlength: 1
    },
    {
      key: 'fax',
      str: 'Факс',
      value: fax,
      handler: setFax,
      required: false,
      type: 'text',
      minlength: 1
    }
  ];

  const handleTextInput = event => {
    const field = fields.find(f => f.key === event.target.id);
    field.handler(event.target.value);
  };

  const handleSelectInput = event => {
    setAcademicDegree(parseInt(event.target.value));
  };

  return (
    <Card>
      <Form id='sign-up-form' onSubmit={e => e.preventDefault()}>
        <FormGroup>
          <Title>Регистрация в системе</Title>
          <Button type='button' onClick={() => navigate('/')}>
            На главную
          </Button>
          <Line />
          {fields.map(f => (
            <InputText
              placeholder={f.str}
              id={f.key}
              key={f.key}
              onChange={handleTextInput}
              required={f.required}
              type={f.type}
              minlength={f.minlength}
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
              Регистрация
            </Button>
          </ButtonWrap>
        </FormGroup>
        {error && <InfoText>{error}</InfoText>}
      </Form>
    </Card>
  );
};

export default SignUp;
