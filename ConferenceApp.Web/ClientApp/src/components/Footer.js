import React from 'react';
import styled from 'styled-components';

const FooterWrapper = styled.section`
  display: flex;
  justify-content: center;
  align-items: center;
  height: 33px;
  background: #000000;

  h2 {
    font-family: 'Montserrat', sans-serif;
    font-size: 15px;
    font-weight: 400;
    color: #5172bf;
  }
`;

const getYear = () => {
  return new Date().getFullYear();
};

const Footer = () => {
  return (
    <FooterWrapper>
      <h2>&copy; {`${getYear()} - Все права защищены`}</h2>
    </FooterWrapper>
  );
};

export default Footer;
