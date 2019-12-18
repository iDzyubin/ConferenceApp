import React, { useEffect, useState } from 'react';

import { Link, animateScroll as scroll } from 'react-scroll';
import Navbar from 'react-bootstrap/Navbar';
import Nav from 'react-bootstrap/Nav';
import Container from 'react-bootstrap/Container';
import styled from 'styled-components';
import kafedralogo from '../assets/img/kafedralogo.png';
import { A } from 'hookrouter';
import { tokensStorage } from '../services/tokensStorage';

const Logo = styled.img`
  width: 50px;
  height: 50px;
`;

const Header = () => {
  const [auth, setAuth] = useState(null);
  useEffect(() => {
    // TODO сделапть проверку на рефреш
    const t = tokensStorage.get();
    if (t) {
      setAuth(t);
    }
  }, []);

  const scrollTo = id => e => {
    e.preventDefault();
    scroll.scrollTo({
      duration: 1500,
      delay: 100,
      smooth: 'easeInOutQuint',
      containerId: id
    });
  };

  return (
    <header>
      <Navbar bg='none' expand='lg' fixed='top'>
        <Container>
          <Navbar.Brand
            href='#home'
            onClick={scrollTo('home')}
            aria-label='Logo'>
            <Logo src={kafedralogo} />
          </Navbar.Brand>
          <Navbar.Toggle aria-controls='basic-navbar-nav'>
            <span />
            <span />
            <span />
          </Navbar.Toggle>
          <Navbar.Collapse id='basic-navbar-nav'>
            <Nav>
              <Link
                href='#'
                className='nav-link'
                activeClass='active'
                to='home'
                spy={true}
                smooth={true}
                offset={0}
                duration={400}>
                Приветствие
              </Link>
              <Link
                href='#'
                className='nav-link'
                activeClass='active'
                to='goals'
                spy={true}
                smooth={true}
                offset={0}
                duration={400}>
                Цели
              </Link>
              <Link
                href='#'
                className='nav-link'
                activeClass='active'
                to='about'
                spy={true}
                smooth={true}
                offset={0}
                duration={400}>
                О конференции
              </Link>
              <Link
                href='#'
                className='nav-link'
                activeClass='active'
                to='digests'
                spy={true}
                smooth={true}
                offset={0}
                duration={400}>
                Сборники прошлых лет
              </Link>
              <Link
                href='#'
                className='nav-link'
                activeClass='active'
                to='photo-archive'
                spy={true}
                smooth={true}
                offset={0}
                duration={400}>
                Фотоархив
              </Link>
              {auth ? (
                <A href='personal-page' className='nav-link'>
                  Личный кабинет
                </A>
              ) : (
                <A href='signin' className='nav-link'>
                  Вход/Регистрация
                </A>
              )}
            </Nav>
          </Navbar.Collapse>
        </Container>
      </Navbar>
    </header>
  );
};

export default Header;
