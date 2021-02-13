import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';

export class AddPhotographerEntry extends Component {
    constructor(props) {
        super(props);

        this.addPhotographer = this.addPhotographer.bind(this);
        this.resultChosen = this.resultChosen.bind(this);
    }

    addPhotographer() {
        alert('this is where we should show search.');

        // TODO: create search in modal, get real data for photographer

        // NOTE: the modal should make these calls.. just here for testing
        var newPhotographer = { "id": "", "firstName": "Bob", "lastName": "Barker", "competitionNumber": "1234", "isDeleted": false };
        this.resultChosen(newPhotographer);
    }

    resultChosen(photographer) {
        this.props.addPhotographer(photographer);
    }

    render() {
        return (
            <Row>
                <Col className="modal-header">
                    <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.addPhotographer(); }}>Add Photographer</button>
                </Col>
            </Row>
        );
    }
}