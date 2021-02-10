import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { CompetitionModal } from './CompetitionModal';

export class Competitions extends Component {
    static displayName = Competitions.name;

    constructor(props) {
        super(props);
        this.state = {
            competitionData: [],
            error: false,
            loading: true,
            errorMessage: "",
            isModalVisible: false
        }

        this.showModal = this.showModal.bind(this);
        this.hideModal = this.hideModal.bind(this);
        this.handleSave = this.handleSave.bind(this);
    }

    componentDidMount() {
        this.getCompetitionData();
    }

    showModal = (competition) => {
        if (competition === null) {
            competition = { "name": "", "date": new Date(), "hasDigital": false, "hasPrint": false };
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

    handleSave = (competition) => {
        if (competition.id !== null) {
            // TODO: put api, and this.hideModal();

            var competitionToUpdate = this.state.competitionData.find(c => c.id === competition.id);
            competitionToUpdate.name = competition.name;
            competitionToUpdate.date = competition.date;
            competitionToUpdate.hasDigital = competition.hasDigital;
            competitionToUpdate.hasPrint = competition.hasPrint;
        }
        else {
            // TODO: post api and update the id in the competition; and this.hideModal();

            this.state.competitionData.push(competition);
        }
    }

    getCompetitionData() {
        var url = process.env.REACT_APP_API_URL + 'GetCompetitions';

        fetch(url,
            {
                method: "GET"
            })
            .then(response => response.json())
            .then(
                (result) => {
                    this.setState({
                        loading: false,
                        error: false,
                        errorMessage: null,
                        competitionData: result === null ? [] : result,
                        currentCompetition: result === null ? [] : result[0]
                    })
                },
                (error) => {
                    console.log(error);
                    this.setState({
                        loading: false,
                        error: true,
                        errorMessage: error,
                        competitionData: null
                    });
                }
            );
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
                                                <a className="btn btn-link" href={"/CompetitionPhotographers?competitionId=" + competition.id}>Photographers</a>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/Scores?competitionId=" + competition.id}>Scores</a>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/Present?competitionId=" + competition.id}>Present</a>
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