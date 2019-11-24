export const SignIn = async user => {
  const response = await fetch("/api/account/signin", {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json',
      Accept: 'application/json',
    },
    body: JSON.stringify(user),
  });
  const data = await response.json();
  console.log(data);
};