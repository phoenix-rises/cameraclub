import React, { Component } from 'react';
import { Row, Col } from 'reactstrap';
import { PhotographerSearchModal } from './PhotographerSearchModal';

export class AddPhotographerEntry extends Component {
    constructor(props) {
        super(props);

        this.state = { isModalVisible: false };

        this.addPhotographer = this.addPhotographer.bind(this);
        this.resultChosen = this.resultChosen.bind(this);
        this.hideModal = this.hideModal.bind(this);
    }

    addPhotographer() {
        this.setState({ isModalVisible: true });
    }

    resultChosen(photographer) {
        this.props.addPhotographer(photographer);
        this.hideModal();
    }

    hideModal() {
        this.setState({ isModalVisible: false });
    }

    render() {
        return (
            <>
                <Row>
                    <Col className="modal-header">
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.addPhotographer(); }}>Add Photographer</button>
                    </Col>
                </Row>
                <PhotographerSearchModal resultChosen={this.resultChosen} show={this.state.isModalVisible} handleClose={this.hideModal} />
            </>
        );
    }
}