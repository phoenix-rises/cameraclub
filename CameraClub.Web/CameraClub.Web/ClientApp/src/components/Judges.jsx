import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { JudgesModal } from './JudgesModal';
import { ClubApi } from '../ClubApi';

export class Judges extends Component {
    clubApi;

    constructor(props) {
        super(props);
        this.state = {
            judgeData: [],
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
        this.getJudgeData();
    }

    showModal = (judge) => {
        if (judge === null) {
            judge = { "id": null, "firstName": "", "lastName": "", "phoneNumber": "", "email": "", "bio": "" };
        }

        this.setState(
            {
                isModalVisible: true,
                currentJudge: judge
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
            userApprovals: null,
            isModalVisible: false
        });
    }

    translate(judge) {
        if (judge.id !== null) {
            var judgeToUpdate = this.state.judgeData.find(c => c.id === judge.id);
            judgeToUpdate.firstName = judge.firstName;
            judgeToUpdate.lastName = judge.lastName;
            judgeToUpdate.email = judge.email;
            judgeToUpdate.bio = judge.bio;
            judgeToUpdate.phoneNumber = judge.phoneNumber;
        }
        else {
            this.state.judgeData.push(judge);
        }

        this.hideModal();
    }

    loadState(judges) {
        this.setState({
            loading: false,
            error: false,
            errorMessage: null,
            judgeData: judges,
            currentJudge: judges[0]
        });
    }

    handleSave = (judge) => {
        this.clubApi.save("UpsertJudges", judge, this.translate, this.hideModal, this.showError);
    }

    getJudgeData() {
        this.clubApi.load("GetJudges", this.showError, this.loadState);
    }

    renderJudges() {
        return (
            <>
                <Row>
                    <Col>
                        <h1 className="page-title">Judges</h1>
                    </Col>
                </Row>
                <Row>
                    <Col className="text-right">
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.showModal(null); }}>Add Judge</button>
                    </Col>
                </Row>
                <Row>
                    {this.state.judgeData.map(judge =>
                        <Container key={judge.id} className="bs-callout bs-callout-info">
                            <Row>
                                <Col>
                                    <h4 className="info">{judge.firstName + " " + judge.lastName}</h4>
                                </Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col sm={3}>Email</Col>
                                <Col sm={9}><a href={"mailto:" + judge.email}>{judge.email}</a></Col>
                            </Row>
                            <Row>
                                <Col sm={3}>Phone Number</Col>
                                <Col sm={9}>{judge.phoneNumber}</Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col sm={3}>Biography</Col>
                                <Col sm={9}>{judge.bio}</Col>
                            </Row>
                            <Row className="top-margin-spacing">
                                <Col>
                                    <button className="btn btn-link" onClick={(e) => { e.preventDefault(); this.showModal(judge); }}>Edit</button>
                                </Col>
                            </Row>
                        </Container>
                    )}
                </Row>
                <JudgesModal handleClose={this.hideModal} handleSave={this.handleSave} show={this.state.isModalVisible} judgeData={this.state.currentJudge} />
            </>
        );
    }


    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderJudges();

        return (
            <div>
                {contents}
            </div>
        );
    }
}