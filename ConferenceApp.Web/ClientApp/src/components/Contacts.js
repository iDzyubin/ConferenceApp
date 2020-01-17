import React from 'react';
import styled from 'styled-components';

import phoneIcon from '../assets/img/phone.png';
import emailIcon from '../assets/img/email.png';
import geoIcon from '../assets/img/geolocation.png';

const ContactsWrapper = styled.section`
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 160px;
  background: #1a1a1a;

  @media (max-width: 768px) {
    height: 100%;
    min-height: 508px;
    padding: 20px;
  }
`;

const Details = styled.div`
  display: flex;
  flex-wrap: wrap;
  justify-content: space-around;
  width: 50%;

  img {
    min-width: 48px;
  }

  @media (max-width: 650px) {
    flex-direction: column;
    width: 85%;
    margin-bottom: 20px;
  }
`;

const Contact = styled.div`
  display: flex;
  align-items: center;
  font-family: 'Montserrat', sans-serif;
  font-size: 15px;

  a {
    margin-left: 25px;
  }

  a:link,
  a:visited,
  a:hover,
  a:active {
    text-decoration: none;
    color: #55565b;
  }

  &:hover a {
    color: chocolate;
  }

  @media (max-width: 650px) {
    margin-bottom: 20px;
  }

  @media (max-width: 450px) {
    a {
      font-size: 13px;
    }
  }
`;

const Icon = styled.img``;

const ContactText = styled.a`
  margin-left: 0;
`;

const Contacts = () => {
  return (
    <ContactsWrapper id="contacts">
      <Details>
        <Contact>
          <Icon src={emailIcon} alt="Mail Icon" />
          <ContactText href="mailto:kafedra-ipm@mail.ru ">
            kafedra-ipm@mail.ru{' '}
          </ContactText>
        </Contact>
        <Contact>
          <Icon src={phoneIcon} alt="Phone Icon" />
          <ContactText href="tel:+7(4712)222673">
            (4712) 22-26-73 - Учёный секретарь Аникина Елена Игоревна, , доцент
            кафедры программной инженерии ЮЗГУ
          </ContactText>
        </Contact>
        <Contact>
          <Icon src={geoIcon} alt="Map Icon" />
          <ContactText href="https://yandex.ru/maps/-/CKAKYW-1" target="_blank">
            305040, г. Курск, ул. 50 лет Октября, д.94, ЮЗГУ, кафедра
            программной инженерии
          </ContactText>
        </Contact>
      </Details>
    </ContactsWrapper>
  );
};

export default Contacts;
