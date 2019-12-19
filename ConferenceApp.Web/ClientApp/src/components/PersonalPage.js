import React, { useEffect, useState } from 'react';
import styled from 'styled-components';

import ModeratorForm from './ModeratorForm';
import UserReports from './UserReports';
import ReportForm from './ReportForm';
import { navigate } from 'hookrouter';
import { tokensStorage } from '../services/tokensStorage';
import EditUser from './EditUser';

const Card = styled.div`
  box-shadow: 0 4px 8px 0 rgba(0, 0, 0, 0.2);
  max-width: 99%;
  margin: auto;
  margin-top: 30px;
  text-align: center;
  font-family: arial;
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

const ButtonWrap = styled.div`
  margin-top: 10px;
  display: flex;
  justify-content: center;
  margin-left: auto;
  margin-right: auto;
  margin-bottom: 20px;
  width: 80%;
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

const PersonalPage = () => {
  const [auth, setAuth] = useState(null);
  const [admin, setAdmin] = useState(false);
  useEffect(() => {
    // TODO сделапть проверку на рефреш
    const t = tokensStorage.get();
    if (t) {
      setAuth(t);
      if (t.role) {
        setAdmin(true);
      }
    } else {
      navigate('/signin');
    }
  }, []);

  const logOut = () => {
    // TODO удаление токена на сервере
    tokensStorage.remove();
    navigate('/signin');
  };

  return (
    auth && (
      <Card>
        <Title>Личный кабинет</Title>
        <ButtonWrap>
          <Button type='button' onClick={() => navigate('/')}>
            На главную
          </Button>
          <EditUser />
          <Button
            style={{ backgroundColor: 'red' }}
            type='button'
            onClick={logOut}>
            Выйти
          </Button>
        </ButtonWrap>
        {!admin ? (
          <div>
            <UserReports reports={[]} />
            <ReportForm
              userId={auth.userId}
              token={auth.jsonWebToken.accessToken}
            />
          </div>
        ) : (
          <ModeratorForm />
        )}
      </Card>
    )
  );
};

export default PersonalPage;
