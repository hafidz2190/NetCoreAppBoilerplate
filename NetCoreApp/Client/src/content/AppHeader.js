/* eslint-disable import/no-extraneous-dependencies */
import PropTypes from 'prop-types';
import React from 'react';
import { Button, Avatar, Typography, Popover, Badge } from 'antd';
import { UserOutlined, DashboardOutlined, NotificationOutlined, SyncOutlined } from '@ant-design/icons';

const { Text } = Typography;

function AppHeader({
  notificationActive,
  notificationPopoverVisible,
  onNotificationPopoverVisibleChange,
  onNotificationRefresh,
}) {
  return (
    <div style={{ height: '100%', display: 'flex', justifyContent: 'space-between' }}>
      <div style={{ display: 'flex', alignItems: 'center' }}>
        <DashboardOutlined style={{ color: '#fff', fontSize: '2.5em' }} />
        <Text style={{ marginLeft: '10px', color: '#fff', fontSize: '1.5em' }}>Dashboard</Text>
      </div>
      <div style={{ display: 'flex', alignItems: 'center' }}>
        <Avatar
          style={{ backgroundColor: 'green' }}
          icon={<UserOutlined />}
        />
        <Text style={{ marginLeft: '10px', color: '#fff' }}>User</Text>
        <Badge
          dot
          count={notificationActive ? 1 : 0}
        >
          <Popover
            visible={notificationPopoverVisible}
            placement="bottom"
            trigger="click"
            content={(
              <div>
                {
                  notificationActive && (
                    <>
                      <Text>User data updated. </Text>
                      <Button
                        type="primary"
                        shape="circle"
                        size="small"
                        icon={<SyncOutlined />}
                        onClick={onNotificationRefresh}
                      />
                    </>
                  )
                }
                {
                  !notificationActive && (
                    <Text>No new notifications.</Text>
                  )
                }
              </div>
            )}
            onVisibleChange={(e) => { onNotificationPopoverVisibleChange(e); }}
          >
            <NotificationOutlined
              style={{ marginLeft: '20px', color: '#fff', fontSize: '1.2em', cursor: 'pointer' }}
              onClick={() => { onNotificationPopoverVisibleChange(true); }}
            />
          </Popover>
        </Badge>
      </div>
    </div>
  );
}

AppHeader.propTypes = {
  notificationActive: PropTypes.bool,
  notificationPopoverVisible: PropTypes.bool,
  onNotificationPopoverVisibleChange: PropTypes.func,
  onNotificationRefresh: PropTypes.func,
};

AppHeader.defaultProps = {
  notificationActive: false,
  notificationPopoverVisible: false,
  onNotificationPopoverVisibleChange: () => {},
  onNotificationRefresh: () => {},
};

export default AppHeader;
