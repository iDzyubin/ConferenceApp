import React, { useEffect } from 'react';

import Login from './Login';
import AdminTable from './AdminTable';
import useGlobal from '../store';

import { tokensStorage } from '../services/tokensStorage'

const Auth = () => {
  const [globalState, globalActions] = useGlobal();
  useEffect(() => {
    // TODO сделапть проверку на рефреш
    if (tokensStorage.get()) {
      globalActions.setAuth(true);
    }
  }, []);
  return (
    globalState.authFlag ? <AdminTable></AdminTable> : <Login></Login>
  );
};

export default Auth;