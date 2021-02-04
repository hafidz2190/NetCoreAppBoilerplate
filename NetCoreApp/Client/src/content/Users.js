/* eslint-disable react-hooks/exhaustive-deps */
/* eslint-disable import/no-extraneous-dependencies */
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Row, Col, Table, Button, Divider, Tooltip, Spin, Modal, Form, Input } from 'antd';
import { PlusOutlined, DeleteOutlined, EditOutlined } from '@ant-design/icons';

import { createUser, fetchUsers, updateUser, deleteUsers } from '../redux/business/userBusiness';

const DATA_SOURCE = [
];

const COLUMN_DEFINITIONS = [
  {
    title: 'Username',
    dataIndex: 'username',
    key: 'username',
  },
  {
    title: 'Name',
    dataIndex: 'name',
    key: 'name',
  },
  {
    title: 'Email',
    dataIndex: 'email',
    key: 'email',
  },
];

const ADD_USER_FORM_LAYOUT = {
  labelCol: { span: 5 },
  wrapperCol: { span: 18 },
};

const mapDispatchToProps = (dispatch) => ({
  userBusinessCreateUser: bindActionCreators(createUser, dispatch),
  userBusinessFetchUsers: bindActionCreators(fetchUsers, dispatch),
  userBusinessUpdateUser: bindActionCreators(updateUser, dispatch),
  userBusinessDeleteUsers: bindActionCreators(deleteUsers, dispatch),
});

function Users({
  contentHeight,
  userBusinessCreateUser,
  userBusinessFetchUsers,
  userBusinessUpdateUser,
  userBusinessDeleteUsers,
}) {
  const [loading, setLoading] = useState(false);
  const [dataSource, setDataSource] = useState(DATA_SOURCE);
  const [selectedRowKeys, setSelectedRowKeys] = useState([]);
  const [userModalVisible, setUserModalVisible] = useState(false);
  const [userModalTitle, setUserModalTitle] = useState(null);
  const [userModalLoading, setUserModalLoading] = useState(false);

  const [form] = Form.useForm();

  const fetchData = async () => {
    setLoading(true);

    const users = await userBusinessFetchUsers();

    setDataSource(users);
    setLoading(false);
  };

  useEffect(() => {
    const init = async () => {
      await fetchData();
    };

    init();
  }, []);

  const addButtonClickHandler = () => {
    form.resetFields();

    setUserModalTitle('Add User');
    setUserModalVisible(true);
  };

  const editButtonClickHandler = () => {
    const selectedDataId = selectedRowKeys[0];
    const selectedData = dataSource.filter((e) => e.id === selectedDataId)[0];
    const { username, name, email } = selectedData;

    form.setFields([
      { name: 'username', value: username },
      { name: 'name', value: name },
      { name: 'email', value: email },
    ]);

    setUserModalTitle('Edit User');
    setUserModalVisible(true);
  };

  const deleteButtonClickHandler = async () => {
    setLoading(true);

    const response = await userBusinessDeleteUsers(selectedRowKeys);

    if (!response) {
      setLoading(false);

      return;
    }

    await fetchData();

    setSelectedRowKeys([]);
  };

  const userModalOkHandler = async () => {
    try {
      await form.validateFields();

      const user = {
        Username: form.getFieldValue('username'),
        Name: form.getFieldValue('name'),
        Email: form.getFieldValue('email'),
      };

      setUserModalLoading(true);

      let createdUser = null;

      if (userModalTitle === 'Add User') {
        createdUser = await userBusinessCreateUser(user);
      } else {
        const selectedDataId = selectedRowKeys[0];

        await userBusinessUpdateUser(selectedDataId, user);
      }

      setUserModalLoading(false);
      setUserModalVisible(false);

      await fetchData();

      if (createdUser) {
        setSelectedRowKeys([createdUser.id]);
      }
    } catch (err) {
      console.log(err);
    }
  };

  const userModalCancelHandler = () => {
    setUserModalVisible(false);
  };

  const onTableSelectChange = (keys) => {
    setSelectedRowKeys(keys);
  };

  const tableRowSelection = {
    selectedRowKeys,
    onChange: onTableSelectChange,
  };

  return (
    <>
      <Row>
        <Tooltip
          placement="bottom"
          title="Add User"
        >
          <Button
            type="primary"
            icon={<PlusOutlined />}
            disabled={loading}
            onClick={addButtonClickHandler}
          />
        </Tooltip>
        <Tooltip
          placement="bottom"
          title="Edit User"
        >
          <Button
            type="secondary"
            icon={<EditOutlined />}
            style={{ marginLeft: '8px' }}
            disabled={loading || selectedRowKeys.length !== 1}
            onClick={editButtonClickHandler}
          />
        </Tooltip>
        <Tooltip
          placement="bottom"
          title="Delete User"
        >
          <Button
            type="danger"
            icon={<DeleteOutlined />}
            style={{ marginLeft: '8px' }}
            disabled={loading || !selectedRowKeys.length}
            onClick={deleteButtonClickHandler}
          />
        </Tooltip>
      </Row>
      <Divider style={{ margin: '10px 0' }} />
      <Row>
        <Col flex="1">
          {
            (loading || !contentHeight) && (
              <div style={{ height: contentHeight - 63, display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                <Spin size="large" />
              </div>
            )
          }
          {
            !loading && contentHeight !== null && (
              <Table
                dataSource={dataSource}
                columns={COLUMN_DEFINITIONS}
                scroll={{ y: contentHeight - 176 }}
                rowSelection={tableRowSelection}
                rowKey="id"
              />
            )
          }
        </Col>
      </Row>
      {
        userModalVisible && (
          <Modal
            title={userModalTitle}
            visible={userModalVisible}
            okButtonProps={{ disabled: userModalLoading }}
            cancelButtonProps={{ disabled: userModalLoading }}
            onOk={userModalOkHandler}
            onCancel={userModalCancelHandler}
          >
            {
              userModalLoading && (
                <div style={{ textAlign: 'center' }}>
                  <Spin size="large" />
                </div>
              )
            }
            {
              !userModalLoading && (
                <Form
                  {...ADD_USER_FORM_LAYOUT}
                  form={form}
                  name="add-user-form"
                >
                  <Form.Item
                    label="Username"
                    name="username"
                    rules={[{ required: true }]}
                  >
                    <Input />
                  </Form.Item>
                  <Form.Item
                    label="Name"
                    name="name"
                    rules={[{ required: true }]}
                  >
                    <Input />
                  </Form.Item>
                  <Form.Item
                    label="Email"
                    name="email"
                    rules={[{ required: true }]}
                  >
                    <Input />
                  </Form.Item>
                </Form>
              )
            }
          </Modal>
        )
      }
    </>
  );
}

Users.propTypes = {
  contentHeight: PropTypes.any.isRequired,
  userBusinessCreateUser: PropTypes.func.isRequired,
  userBusinessFetchUsers: PropTypes.func.isRequired,
  userBusinessUpdateUser: PropTypes.func.isRequired,
  userBusinessDeleteUsers: PropTypes.func.isRequired,
};

export default connect(null, mapDispatchToProps)(Users);
