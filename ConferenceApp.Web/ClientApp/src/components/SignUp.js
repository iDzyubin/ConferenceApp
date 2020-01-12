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
  color: #5172bf;
`;

const ErrorText = styled.p`
  margin-left: auto;
  margin-right: auto;
  color: red;
`;

const SignUp = () => {
  const [Email, setEmail] = useState('');
  const [Password1, setPassword1] = useState('');
  const [Password2, setPassword2] = useState('');

  const [FirstName, setFirstName] = useState();
  const [MiddleName, setMiddleName] = useState('');
  const [LastName, setLastName] = useState('');
  const [Organisation, setOrganization] = useState('');
  const [Phone, setPhone] = useState('');
  const [OrganisationAddress, setOrganizationAddress] = useState('');
  const [City, setCity] = useState('');
  const [Position, setPosition] = useState('');

  const [error, setError] = useState(null);
  const [msg, setMsg] = useState(null);

  const handleSubmit = async e => {
    let formObj = document.getElementById('sign-up-form');
    if (formObj.checkValidity()) {
      if (Password1 === Password2) {
        const user = {
          Email,
          Password: Password1,
          FirstName,
          MiddleName,
          LastName,
          Organisation,
          Phone,
          OrganisationAddress,
          City,
          Position
        };
        Api.SignUp(user)
          .catch(() =>
            setError('Что-то пошло не так. Обратитесь к администратору')
          )
          .then(response => {
            if (response) {
              setMsg('Проверьте ваш почтовый ящик для продолжения регистрации');
            } else {
              setError('Ошибка регистрации');
            }
          });
      } else {
        setError('Пароли не совпадают');
      }
    }
  };

  const fields = [
    {
      key: 'Email',
      str: 'E-mail',
      value: Email,
      handler: setEmail,
      required: true,
      type: 'email',
      minlength: 5
    },
    {
      key: 'password1',
      str: 'Пароль (минимум 4 символа)',
      value: Password1,
      handler: setPassword1,
      required: true,
      type: 'password',
      minlength: 5
    },
    {
      key: 'password2',
      str: 'Подтверждение пароля',
      value: Password2,
      handler: setPassword2,
      required: true,
      type: 'password',
      minlength: 5
    },
    {
      key: 'FirstName',
      str: 'Имя',
      value: FirstName,
      handler: setFirstName,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'MiddleName',
      str: 'Отчество',
      value: MiddleName,
      handler: setMiddleName,
      required: false,
      type: 'text',
      minlength: 1
    },
    {
      key: 'LastName',
      str: 'Фамилия',
      value: LastName,
      handler: setLastName,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'Organisation',
      str: 'Организация',
      value: Organisation,
      handler: setOrganization,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'Phone',
      str: 'Телефон',
      value: Phone,
      handler: setPhone,
      required: true,
      type: 'number',
      minlength: 1
    },
    {
      key: 'OrganisationAddress',
      str: 'Адрес организации',
      value: OrganisationAddress,
      handler: setOrganizationAddress,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'City',
      str: 'Город организации',
      value: City,
      handler: setCity,
      required: true,
      type: 'text',
      minlength: 1
    },
    {
      key: 'Position',
      str: 'Должность',
      value: Position,
      handler: setPosition,
      required: false,
      type: 'text',
      minlength: 1
    }
  ];

  const handleTextInput = event => {
    const field = fields.find(f => f.key === event.target.id);
    field.handler(event.target.value);
  };

  return (
    <Card>
      <Form id="sign-up-form" onSubmit={e => e.preventDefault()}>
        <FormGroup>
          <Title>Регистрация в системе</Title>
          <Button type="button" onClick={() => navigate('/')}>
            На главную
          </Button>
          <Button type="button" onClick={() => navigate('/signin')}>
            Войти в систему
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
          {error && <ErrorText>{error}</ErrorText>}
          {msg && <InfoText>{msg}</InfoText>}
          <ButtonWrap>
            <Button type="submit" onClick={handleSubmit}>
              Регистрация
            </Button>
          </ButtonWrap>
        </FormGroup>
      </Form>
    </Card>
  );
};

export default SignUp;
