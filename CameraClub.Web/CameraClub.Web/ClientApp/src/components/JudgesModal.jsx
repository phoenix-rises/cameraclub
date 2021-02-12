import React, { Component } from 'react';
import { Container, Row, Col, Label, Input } from 'reactstrap';

export class JudgesModal extends Component {
    constructor(props) {
        super(props);

        this.state = { "id": null, "firstName": "", "lastName": "", "bio": "", "email": "", "phoneNumber": "" };

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
            var data = this.props.judgeData;

            this.setState(
                {
                    id: data.id,
                    firstName: data.firstName,
                    lastName: data.lastName,
                    phoneNumber: data.phoneNumber,
                    email: data.email,
                    bio: data.bio
                });
        }
    }

    render() {
        return (
            <div className={this.props.show ? "modal display-block" : "modal display-none"}>
                <div className="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{this.state.id === null ? "Add Judge" : "Edit Judge"}</h5>
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
                                <Label for="phoneNumber" sm={4}>Phone Number</Label>
                                <Col sm={8}>
                                    <Input type="text" name="phoneNumber" value={this.state.phoneNumber} onChange={this.handleChange} />
                                </Col>
                            </Row>
                            <Row>
                                <Label for="bio" sm={4}>Biography</Label>
                                <Col sm={8}>
                                    <Input type="textarea" name="bio" value={this.state.bio} onChange={this.handleChange} />
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