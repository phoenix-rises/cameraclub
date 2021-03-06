﻿import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { CompetitionModal } from './CompetitionModal';
import { ClubApi } from '../ClubApi';

export class Competitions extends Component {
    clubApi;

    constructor(props) {
        super(props);
        this.state = {
            competitionData: [],
            error: false,
            loading: true,
            errorMessage: "",
            isModalVisible: false
        }

        this.clubApi = new ClubApi();

        this.showModal = this.showModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.loadState = this.loadState.bind(this);
        this.showError = this.showError.bind(this);
        this.translate = this.translate.bind(this);
    }

    componentDidMount() {
        this.getCompetitionData();
    }

    showModal = (competition) => {
        if (competition === null) {
            competition = { "id": null, "name": "", "date": "", "hasDigital": false, "hasPrint": false };
        }

        this.setState(
            {
                isModalVisible: true,
                currentCompetition: competition
            });
    };

    hideModal = () => {
        this.setState({ isModalVisible: false });
    };

    showError(error) {
        console.log(error);
        this.setState({
            loading: false,
            error: true,
            errorMessage: error,
            isModalVisible: false
        });
    }

    translate(competition) {
        if (competition.id !== null) {
            var competitionToUpdate = this.state.competitionData.find(c => c.id === competition.id);
            competitionToUpdate.name = competition.name;
            competitionToUpdate.date = competition.date;
            competitionToUpdate.hasDigital = competition.hasDigital;
            competitionToUpdate.hasPrint = competition.hasPrint;
        }
        else {
            this.state.competitionData.push(competition);
        }

        this.hideModal();

        this.getCompetitionData();
    }

    loadState(competitions) {
        this.setState({
            loading: false,
            error: false,
            errorMessage: null,
            competitionData: competitions,
            currentCompetition: competitions[0]
        });
    }

    handleSave = (competition) => {
        this.clubApi.save("SaveCompetition", competition, this.translate, this.hideModal, this.showError);
    }

    getCompetitionData() {
        this.clubApi.load("GetCompetitions", this.showError, this.loadState);
    }

    renderCompetitions() {
        return (
            <>
                <Row>
                    <Col>
                        <h1 className="page-title">Competitions</h1>
                    </Col>
                </Row>
                <Row>
                    <Col className="text-right">
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.showModal(null); }}>Add Competition</button>
                    </Col>
                </Row>
                <Row>
                    {this.state.competitionData.map(competition =>
                        <Container key={competition.id} className="bs-callout bs-callout-info">
                            <Row>
                                <Col>
                                    <h4 className="info">{competition.name} - {new Intl.DateTimeFormat('en-us').format(new Date(competition.date))}</h4>
                                </Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>Digital included? {competition.hasDigital ? "Yes" : "No"}</Col>
                            </Row>
                            <Row>
                                <Col>Print included? {competition.hasPrint ? "Yes" : "No"}</Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>
                                    <Container>
                                        <Row>
                                            <Col>
                                                <button className="btn btn-link" onClick={(e) => { e.preventDefault(); this.showModal(competition); }}>Edit</button>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/CompetitionPhotographers/" + competition.id}>Photographers</a>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/Scores/" + competition.id}>Scores</a>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/Present/" + competition.id}>Present</a>
                                            </Col>
                                            <Col></Col>
                                            <Col></Col>
                                            <Col></Col>
                                            <Col></Col>
                                            <Col></Col>
                                            <Col></Col>
                                            <Col></Col>
                                            <Col></Col>
                                        </Row>
                                    </Container>
                                </Col>
                            </Row>
                        </Container>
                    )}
                </Row>
                <CompetitionModal handleClose={this.hideModal} handleSave={this.handleSave} show={this.state.isModalVisible} competitionData={this.state.currentCompetition} />
            </>
        );
    }


    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderCompetitions();

        return (
            <div>
                {contents}
            </div>
        );
    }
}