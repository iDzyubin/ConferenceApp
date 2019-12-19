import React, { useEffect, Fragment } from 'react';
import AOS from 'aos';
import $ from 'jquery';

import Header from './components/Header';
import Home from './components/Home';
import About from './components/About';
import Contacts from './components/Contacts';
import Goals from './components/Goals';
import Footer from './components/Footer';
import SignIn from './components/SignIn';
import SignUp from './components/SignUp';
import PersonalPage from './components/PersonalPage';
import NoPageFound from './components/NoPageFound';

import 'aos/dist/aos.css';
import './assets/styles/main.scss';
import PhotoArchive from './components/PhotoArchive';
import Digests from './components/Digests';

import { useRoutes } from 'hookrouter';

const App = () => {
  useEffect(() => {
    AOS.init({ once: true });

    let navElement = $('nav');

    $(function() {
      $(window).scrollTop() > navElement.innerHeight()
        ? navElement.addClass('sticky')
        : navElement.removeClass('sticky');
    });
    $(window).on('scroll', function() {
      $(window).scrollTop() > navElement.innerHeight()
        ? navElement.addClass('sticky')
        : navElement.removeClass('sticky');
    });
  });

  const routes = {
    '/': () => (
      <Fragment>
        <Header />
        <main>
          <Home />
          <Goals />
          <About />
          <Digests />
          <PhotoArchive />
          <Contacts />
          <Footer />
        </main>
      </Fragment>
    ),
    '/signin': () => <SignIn />,
    '/signup': () => <SignUp />,
    '/personal-page': () => <PersonalPage />
  };
  const routeResult = useRoutes(routes);

  return <div>{routeResult || <NoPageFound />}</div>;
};

export default App;
