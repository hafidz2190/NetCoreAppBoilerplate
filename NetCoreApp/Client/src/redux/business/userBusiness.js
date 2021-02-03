import { callRestApi } from '../../helper/api';

export const createUser = (user) => async (dispatch, getState) => {
  const state = getState();
  const { appStore } = state;
  const { restApiBaseUrl } = appStore;

  let newUser = null;

  try {
    const response = await callRestApi('post', restApiBaseUrl, 'user/create', user);

    newUser = response.payload;
  } catch (err) {
    console.log(err);

    return null;
  }

  return newUser;
};

export const fetchUsers = () => async (dispatch, getState) => {
  const state = getState();
  const { appStore } = state;
  const { restApiBaseUrl } = appStore;

  let users = [];

  try {
    const response = await callRestApi('get', restApiBaseUrl, 'user/read');

    users = response.payload;
  } catch (err) {
    console.log(err);

    return [];
  }

  return users;
};

export const updateUser = (id, propertyMap) => async (dispatch, getState) => {
  const state = getState();
  const { appStore } = state;
  const { restApiBaseUrl } = appStore;

  let updatedUser = null;

  try {
    const response = await callRestApi('post', restApiBaseUrl, 'user/update', { Id: id, PropertyMap: propertyMap });

    updatedUser = response.payload;
  } catch (err) {
    console.log(err);

    return null;
  }

  return updatedUser;
};

export const deleteUser = (id) => async (dispatch, getState) => {
  const state = getState();
  const { appStore } = state;
  const { restApiBaseUrl } = appStore;

  let success = false;

  try {
    const response = await callRestApi('post', restApiBaseUrl, 'user/delete', null, { id });

    success = response.payload;
  } catch (err) {
    console.log(err);

    return false;
  }

  return success;
};

export const deleteUsers = (ids) => async (dispatch, getState) => {
  const state = getState();
  const { appStore } = state;
  const { restApiBaseUrl } = appStore;

  let success = false;

  try {
    const response = await callRestApi('post', restApiBaseUrl, 'user/delete', ids);

    success = response.payload;
  } catch (err) {
    console.log(err);

    return false;
  }

  return success;
};
