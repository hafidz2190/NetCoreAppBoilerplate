import { combineReducers } from 'redux';
import appStore from './appStore';

const rootReducer = combineReducers({
  appStore,
});

export default rootReducer;
