import { connectWebSocket } from '../../helper/api';

export const buildHub = () => async (dispatch, getState) => {
  const state = getState();
  const { appStore } = state;
  const { webSocketBaseUrl } = appStore;

  const hub = await connectWebSocket(webSocketBaseUrl, 'main');

  return hub;
};

export default buildHub();
