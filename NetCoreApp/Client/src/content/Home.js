/* eslint-disable react-hooks/exhaustive-deps */
import PropTypes from 'prop-types';
import React, { useState, useEffect } from 'react';
import { connect } from 'react-redux';
import { bindActionCreators } from 'redux';
import { Row, Col, Card, Statistic, Spin } from 'antd';

import { fetchUsers } from '../redux/business/userBusiness';

const mapDispatchToProps = (dispatch) => ({
  userBusinessFetchUsers: bindActionCreators(fetchUsers, dispatch),
});

function Home({
  userBusinessFetchUsers,
}) {
  const [loading, setLoading] = useState(false);
  const [dataSourceLength, setDataSourceLength] = useState(null);

  const fetchData = async () => {
    setLoading(true);

    const users = await userBusinessFetchUsers();

    setDataSourceLength(users.length);
    setLoading(false);
  };

  useEffect(() => {
    const init = async () => {
      await fetchData();
    };

    init();
  }, []);

  return (
    <Row>
      <Col span={5}>
        <Card>
          {
            loading && (
              <div style={{ textAlign: 'center' }}>
                <Spin size="large" />
              </div>
            )
          }
          {
            !loading && (
              <Statistic
                title="Active Users"
                value={dataSourceLength}
              />
            )
          }
        </Card>
      </Col>
    </Row>
  );
}

Home.propTypes = {
  userBusinessFetchUsers: PropTypes.func.isRequired,
};

export default connect(null, mapDispatchToProps)(Home);
