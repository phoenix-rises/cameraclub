import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { Competitions } from './components/Competitions';
import { Photographers } from './components/Photographers';
import { Categories } from './components/Categories';
import { Club } from './components/Club';
import { Judges } from './components/Judges';
import { CompetitionPhotographers } from './components/CompetitionPhotographers';

import './custom.css'

export default class App extends Component {
    static displayName = App.name;

    render() {
        return (
            <Layout>
                <Route exact path='/' component={Competitions} />
                <Route exact path='/Photographers' component={Photographers} />
                <Route exact path='/Categories' component={Categories} />
                <Route exact path='/Club' component={Club} />
                <Route exact path='/Judges' component={Judges} />
                <Route exact path='/CompetitionPhotographers/:competitionId' component={CompetitionPhotographers} />
            </Layout>
        );
    }
}