import React, { useState } from 'react';
import styled from 'styled-components';

import * as Api from '../services/api';
import { localStorage } from '../services/localStorage';
import { A, navigate } from 'hookrouter';

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: space-around;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
  width: 300px;
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
const LabelInput = styled.label`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  color: #1d4dbb;
`;

const Card = styled.div`
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  max-width: 800px;
  margin: auto;
  margin-top: 30px;
  text-align: center;
  font-family: arial;
`;

const InfoText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 10px;
  padding-bottom: 10px;
  font-size: 25px;
  color: blue;
`;

const ErrorText = styled.p`
  margin-right: 5px;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 10px;
  padding-bottom: 10px;
  color: red;
`;

const SignIn = () => {
  const [Email, setEmail] = useState('');
  const [Password, setPassword] = useState('');

  const [error, setError] = useState(null);

  const handleSubmit = async () => {
    let formObj = document.getElementById('sign-in-form');
    if (formObj.checkValidity()) {
      const user = {
        Email,
        Password
      };
      Api.SignIn(user)
        .catch(() =>
          setError('Что-то пошло не так. Обратитесь к администратору')
        )
        .then(data => {
          if (checkRespone(data)) {
            setError(null);
            localStorage.add(data);
            navigate('/personal-page');
          } else {
            setError('Неправильный логин или пароль');
          }
        });
    }
  };

  const checkRespone = resp => {
    return (
      resp.hasOwnProperty('jsonWebToken') &&
      resp.hasOwnProperty('role') &&
      resp.hasOwnProperty('userId') &&
      resp.hasOwnProperty('fullName')
    );
  };

  const handleChangeEmail = event => {
    setEmail(event.target.value);
  };

  const handleChangePassword = event => {
    setPassword(event.target.value);
  };

  return (
    <Card>
      <Form id='sign-in-form' onSubmit={e => e.preventDefault()}>
        <FormGroup>
          <Title>Вход в личный кабинет</Title>
          <Button type='button' onClick={() => navigate('/')}>
            На главную
          </Button>
          <Line />
          <LabelInput htmlFor='email'>
            <b>Эл. адрес</b>
          </LabelInput>
          <InputText
            type='email'
            placeholder='Введите эл. адрес'
            name='email'
            value={Email}
            onChange={handleChangeEmail}
            required
          />
          <LabelInput htmlFor='psw'>
            <b>Пароль</b>
          </LabelInput>
          <InputText
            type='password'
            placeholder='Введите пароль'
            name='psw'
            value={Password}
            onChange={handleChangePassword}
            required
          />
          <ButtonWrap>
            <Button type='submit' onClick={handleSubmit}>
              Вход
            </Button>
          </ButtonWrap>
        </FormGroup>
        {error && <ErrorText>{error}</ErrorText>}
      </Form>
      <InfoText>
        <A href='signup'>Регистрация</A>
      </InfoText>
    </Card>
  );
};

export default SignIn;
