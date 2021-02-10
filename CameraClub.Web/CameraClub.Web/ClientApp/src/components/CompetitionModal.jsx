import React, { Component } from 'react';
import { Container, Row, Col, Form, FormGroup, Label, Input, FormText } from 'reactstrap';

export class CompetitionModal extends Component {
    constructor(props) {
        super(props);

        this.state = props.competitionData === null ? { "name": "", "date": "" } : props.competitionData;

        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        this.setState({
            [name]: value
        });
    }

    render() {
        return (
            <Container className={this.props.show ? "modal display-block" : "modal display-none"}>
                <Row>
                    <Col className="modal-main">
                        <button type="button" className="close" aria-label="Close" onClick={this.props.handleClose}><span aria-hidden="true">&times;</span></button>
                        <Container className="modal-body">
                            <Form onSubmit={this.props.handleSave}>
                                <FormGroup row>
                                    <Label for="name" sm={2}>Name</Label>
                                    <Col sm={10}>
                                        <Input type="text" name="name" placeholder="Name of the competition" value={this.state.name} onChange={this.handleChange} />
                                    </Col>
                                </FormGroup>
                                <FormGroup row>
                                    <Label for="date" sm={2}>Date</Label>
                                    <Col sm={10}>
                                        <Input type="text" name="name" placeholder="i.e. 09/21/1970" value={this.state.date} onChange={this.handleChange} />
                                    </Col>
                                </FormGroup>
                                <FormGroup check>
                                    <Label check for="hasDigital">
                                        <Input type="checkbox" name="hasDigital" id="hasDigital" value={this.state.hasDigital} />
                                        Digital included
                                    </Label>
                                </FormGroup>
                                <FormGroup check>
                                    <Label check for="hasPrint">
                                        <Input type="checkbox" name="hasPrint" id="hasPrint" value={this.state.hasPrint} />
                                        Print included
                                    </Label>
                                </FormGroup>
                                <FormGroup row>
                                    <Col className="text-right"><input className="btn btn-primary" type="submit" value="Save" /></Col>
                                    <Col className="text-left"><button className="btn btn-primary" onClick={this.props.handleClose}>Close</button></Col>
                                </FormGroup>
                            </Form>
                        </Container>
                    </Col>
                </Row>
            </Container>
        );
    }
}