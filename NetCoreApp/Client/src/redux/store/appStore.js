import { SET_SELECTED_SIDE_MENU } from '../../constant/actionTypeEnum';
import { HOME } from '../../constant/sideMenuEnum';

const initialState = {
  selectedSideMenu: HOME,
  restApiBaseUrl: `http://${window.location.host}/api`,
  webSocketBaseUrl: `http://${window.location.host}/hub`,
};

const appStore = (state = initialState, action) => {
  switch (action.type) {
    case SET_SELECTED_SIDE_MENU:
      return { ...state, selectedSideMenu: action.data };
    default:
      return state;
  }
};

export default appStore;
