import * as React from 'react';
import { Route, Switch } from 'react-router';
import Landing from './views/pages/Landing';

export default () => (
    <Switch>
        <Route exact path='/' component={Landing} />
    </Switch>
);
