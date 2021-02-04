/* eslint-disable import/no-extraneous-dependencies */
/* eslint-disable react-hooks/exhaustive-deps */
import PropTypes from 'prop-types';
import React, { useState, useRef, useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Layout, Menu } from 'antd';
import { UserOutlined, HomeOutlined } from '@ant-design/icons';

import AppHeader from './AppHeader';
import Home from './Home';
import Users from './Users';
import './App.scss';

import { HOME, USERS } from '../constant/sideMenuEnum';

import { setSelectedSideMenu } from '../redux/action/appAction';
import { buildHub } from '../redux/business/mainHubBusiness';

const { Content, Sider, Header } = Layout;

const mapStateToProps = (state) => ({
  appStore: state.appStore,
});

const mapDispatchToProps = (dispatch) => ({
  appActionSetSelectedSideMenu: bindActionCreators(setSelectedSideMenu, dispatch),
  mainHubBusinessBuildHub: bindActionCreators(buildHub, dispatch),
});

function App({
  appStore,
  appActionSetSelectedSideMenu,
  mainHubBusinessBuildHub,
}) {
  const [siderCollapsed, setSiderCollapsed] = useState(true);
  const [notificationActive, setNotificationActive] = useState(false);
  const [notificationPopoverVisible, setNotificationPopoverVisible] = useState(false);
  const [notifierDate, setNotifierDate] = useState(null);
  const [contentHeight, setContentHeight] = useState(null);

  const contentRef = useRef();

  const resizeHandler = () => {
    if (!contentRef || !contentRef.current) {
      return;
    }

    setContentHeight(contentRef.current.clientHeight);
  };

  useEffect(() => {
    const init = async () => {
      const hub = await mainHubBusinessBuildHub();

      hub.on('OnUserDataUpdated', (arg) => {
        console.log('OnUserDataUpdated', arg);

        setNotificationActive(true);
      });
    };

    window.addEventListener('resize', resizeHandler);

    init();
    resizeHandler();

    return () => {
      window.removeEventListener('resize', resizeHandler);
    };
  }, []);

  const onSiderCollapse = () => {
    setSiderCollapsed(!siderCollapsed);
  };

  const onSideMenuClick = async (e) => {
    appActionSetSelectedSideMenu(e.key);
  };

  const onNotificationRefresh = async () => {
    setNotificationPopoverVisible(false);

    setNotifierDate(new Date());

    setTimeout(() => {
      setNotificationActive(false);
    }, 200);
  };

  return (
    <div className="app">
      <Layout>
        <Header>
          <AppHeader
            notificationActive={notificationActive}
            notificationPopoverVisible={notificationPopoverVisible}
            onNotificationPopoverVisibleChange={(e) => { setNotificationPopoverVisible(e); }}
            onNotificationRefresh={onNotificationRefresh}
          />
        </Header>
        <Layout>
          <Sider
            collapsible
            collapsed={siderCollapsed}
            onCollapse={onSiderCollapse}
          >
            <Menu
              defaultSelectedKeys={[HOME]}
              mode="inline"
              onClick={onSideMenuClick}
            >
              <Menu.Item
                key={HOME}
                icon={<HomeOutlined />}
              >
                Home
              </Menu.Item>
              <Menu.Item
                key={USERS}
                icon={<UserOutlined />}
              >
                Users
              </Menu.Item>
            </Menu>
          </Sider>
          <Content style={{ height: 'calc(100vh - 64px)', padding: '10px', background: '#ddd' }}>
            <div
              style={{ height: '-webkit-fill-available', padding: '10px', background: '#fff' }}
              ref={contentRef}
            >
              {
                appStore.selectedSideMenu === HOME && (
                  <Home key={notifierDate} />
                )
              }
              {
                appStore.selectedSideMenu === USERS && (
                  <Users
                    key={notifierDate}
                    contentHeight={contentHeight}
                  />
                )
              }
            </div>
          </Content>
        </Layout>
      </Layout>
    </div>
  );
}

App.propTypes = {
  appStore: PropTypes.object.isRequired,
  appActionSetSelectedSideMenu: PropTypes.func.isRequired,
  mainHubBusinessBuildHub: PropTypes.func.isRequired,
};

export default connect(mapStateToProps, mapDispatchToProps)(App);
