import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { PhotographersModal } from './PhotographersModal';

export class Photographers extends Component {
    static displayName = Photographers.name;

    constructor(props) {
        super(props);
        this.state = {
            photographerData: [],
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
        this.getPhotographerData();
    }

    showModal = (photographer) => {
        if (photographer === null) {
            photographer = { "id": null, "firstName": "", "lastName": "", "competitionNumber": "", "email": "", "clubNumber": "" };
        }

        this.setState(
            {
                isModalVisible: true,
                currentPhotographer: photographer
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
            userApprovals: null
        });
    }

    handleSave = (photographer) => {
        var url = process.env.REACT_APP_API_URL + "UpsertPhotographer";

        if (photographer.id !== null) {
            fetch(url,
                {
                    method: "PUT",
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({
                        id: photographer.id,
                        firstName: photographer.firstName,
                        lastName: photographer.lastName,
                        competitionNumber: photographer.competitionNumber,
                        email: photographer.email,
                        clubNumber: photographer.clubNumber
                    })
                })
                .then(
                    (result) => {
                        var photographerToUpdate = this.state.photographerData.find(c => c.id === photographer.id);
                        photographerToUpdate.firstName = photographer.firstName;
                        photographerToUpdate.lastName = photographer.lastName;
                        photographerToUpdate.email = photographer.email;
                        photographerToUpdate.competitionNumber = photographer.competitionNumber;
                        photographerToUpdate.clubNumber = photographer.clubNumber;

                        this.hideModal();
                    },
                    (error) => {
                        this.hideModal();
                        this.showError(error);
                    }
                );
        }
        else {
            fetch(url,
                {
                    method: "POST",
                    headers: {
                        'Content-type': 'application/json'
                    },
                    body: JSON.stringify({
                        firstName: photographer.firstName,
                        lastName: photographer.lastName,
                        competitionNumber: photographer.competitionNumber,
                        email: photographer.email,
                        clubNumber: photographer.clubNumber
                    })
                })
                .then(
                    (result) => {
                        this.state.photographerData.push(photographer);

                        this.hideModal();
                    },
                    (error) => {
                        this.hideModal();
                        this.showError(error);
                    }
                );
        }
    }

    getPhotographerData() {
        var url = process.env.REACT_APP_API_URL + 'GetPhotographers';

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
                        photographerData: result,
                        currentPhotographer: result[0]
                    })
                },
                (error) => {
                    this.showError(error);
                }
            );
    }

    renderPhotographers() {
        return (
            <>
                <Row>
                    <Col>
                        <h1 className="page-title">Photographers</h1>
                    </Col>
                </Row>
                <Row>
                    <Col className="text-right">
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.showModal(null); }}>Add Photographer</button>
                    </Col>
                </Row>
                <Row>
                    {this.state.photographerData.map(photographer =>
                        <Container key={photographer.id} className="bs-callout bs-callout-info">
                            <Row>
                                <Col>
                                    <h4 className="info">{photographer.firstName + " " + photographer.lastName}</h4>
                                </Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col sm={3}>Email</Col>
                                <Col sm={9}><a href={"mailto:" + photographer.email}>{photographer.email}</a></Col>
                            </Row>
                            <Row>
                                <Col sm={3}>Competition Number</Col>
                                <Col sm={9}>{photographer.competitionNumber}</Col>
                            </Row>
                            <Row>
                                <Col sm={3}>Club Number</Col>
                                <Col sm={9}>{photographer.clubNumber}</Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>
                                    <button className="btn btn-link" onClick={(e) => { e.preventDefault(); this.showModal(photographer); }}>Edit</button>
                                </Col>
                            </Row>
                        </Container>
                    )}
                </Row>
                <PhotographersModal handleClose={this.hideModal} handleSave={this.handleSave} show={this.state.isModalVisible} photographerData={this.state.currentPhotographer} />
            </>
        );
    }


    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderPhotographers();

        return (
            <div>
                {contents}
            </div>
        );
    }
}