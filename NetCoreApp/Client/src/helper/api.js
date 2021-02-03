import axios from 'axios';

const signalR = require('@microsoft/signalr');

export const callRestApi = async (method, url, route, data, params, responseType) => {
  const response = await axios({
    method,
    url: `${url}/${route}`,
    data,
    params,
    responseType,
  });

  console.group('axios');
  console.log('host', url);
  console.log('route', route);
  console.log('method', method);
  console.log('data', data);
  console.log('params', params);
  console.log('responseType', responseType);
  console.log('response', response.data);
  console.groupEnd();

  return response.data;
};

export const connectWebSocket = async (url, route) => {
  try {
    const hub = new signalR.HubConnectionBuilder().withUrl(`${url}/${route}`).withAutomaticReconnect([0, 1000, 2000, 3000, 4000, 5000]).build();
    hub.serverTimeoutInMilliseconds = 30000;

    await hub.start();

    return hub;
  } catch (err) {
    console.log(err);

    return null;
  }
};
