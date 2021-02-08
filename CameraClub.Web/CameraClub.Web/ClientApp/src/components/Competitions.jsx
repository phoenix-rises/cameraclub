import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { ApiInterface } from '../ApiInterface';

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
    }

    componentDidMount() {
        this.getCompetitionData();
    }

    showModal = () => {
        this.setState({ isModalVisible: true });
    };

    hideModal = () => {
        this.setState({ isModalVisible: false });
    };

    getCompetitionData() {
        var url = ApiInterface.apiUrl;

        // TODO: fetch from API

        this.setState({
            loading: false,
            error: false,
            competitionData:
                [
                    { "Id": "0", "Name": "Spring Extravaganza", "Date": "03/01/2021", "HasDigital": true, "HasPrint": false },
                    { "Id": "1", "Name": "Fall Extravaganza", "Date": "09/01/2021", "HasDigital": true, "HasPrint": true }
                ]
        })
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
                        <button className="btn btn-primary">Add Competition</button>
                    </Col>
                </Row>
                <Row>
                    {this.state.competitionData.map(competition =>
                        <Container key={competition.Id} className="bs-callout bs-callout-info">
                            <Row>
                                <Col>
                                    <h4 className="info">{competition.Name} - {competition.Date}</h4>
                                </Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>Digital included? {competition.HasDigital ? "Yes" : "No"}</Col>
                            </Row>
                            <Row>
                                <Col>Print included? {competition.HasPrint ? "Yes" : "No"}</Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>
                                    <Container>
                                        <Row>
                                            <Col>
                                                <button className="btn btn-link" onClick={(e) => { e.preventDefault(); alert('modal should open'); }}>Edit</button>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/CompetitionPhotographers?competitionId=" + competition.Id}>Photographers</a>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/Scores?competitionId=" + competition.Id}>Scores</a>
                                            </Col>
                                            <Col>
                                                <a className="btn btn-link" href={"/Present?competitionId=" + competition.Id}>Present</a>
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