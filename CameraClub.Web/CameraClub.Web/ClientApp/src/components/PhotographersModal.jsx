import React, { Component } from 'react';
import { Container, Row, Col, Label, Input } from 'reactstrap';

export class PhotographersModal extends Component {
    constructor(props) {
        super(props);

        this.state = { "id": null, "firstName": "", "lastName": "", "competitionNumber": "", "email": "", "clubNumber": "" };

        this.handleChange = this.handleChange.bind(this);
        this.handleSave = this.handleSave.bind(this);
        this.handleClose = this.handleClose.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    handleSave() {
        this.props.handleSave(this.state);
    }

    handleClose() {
        this.props.handleClose();
    }

    componentDidUpdate(prevProps) {
        if (!prevProps.show && this.props.show) {
            var data = this.props.photographerData;

            this.setState(
                {
                    id: data.id,
                    firstName: data.firstName,
                    lastName: data.lastName,
                    competitionNumber: data.competitionNumber,
                    email: data.email,
                    clubNumber: data.clubNumber
                });
        }
    }

    render() {
        return (
            <div className={this.props.show ? "modal display-block" : "modal display-none"}>
                <div className="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{this.state.id === null ? "Add Photographer" : "Edit Photographer"}</h5>
                            <button type="button" className="close" aria-label="Close" onClick={this.props.handleClose}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <Container className="modal-body">
                            <Row>
                                <Label for="firstName" sm={4}>First Name</Label>
                                <Col sm={8}>
                                    <Input type="text" name="firstName" value={this.state.firstName} onChange={this.handleChange} />
                                </Col>
                            </Row>
                            <Row>
                                <Label for="lastName" sm={4}>Last Name</Label>
                                <Col sm={8}>
                                    <Input type="text" name="lastName" value={this.state.lastName} onChange={this.handleChange} />
                                </Col>
                            </Row>
                            <Row>
                                <Label for="email" sm={4}>Email</Label>
                                <Col sm={8}>
                                    <Input type="text" name="email" value={this.state.email} onChange={this.handleChange} />
                                </Col>
                            </Row>
                            <Row>
                                <Label for="competitionNumber" sm={4}>Competition Number</Label>
                                <Col sm={8}>
                                    <Input type="text" name="competitionNumber" value={this.state.competitionNumber} onChange={this.handleChange} />
                                </Col>
                            </Row>
                            <Row>
                                <Label for="clubNumber" sm={4}>Club Number</Label>
                                <Col sm={8}>
                                    <Input type="text" name="clubNumber" value={this.state.clubNumber} onChange={this.handleChange} />
                                </Col>
                            </Row>
                        </Container>
                        <div className="modal-footer">
                            <button className="btn btn-secondary" onClick={this.handleClose}>Close</button>
                            <button className="btn btn-primary" onClick={this.handleSave}>Save Changes</button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}