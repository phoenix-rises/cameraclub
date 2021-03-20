import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { ClubApi } from '../ClubApi';

export class PhotoModal extends Component {
    clubApi;

    constructor(props) {
        super(props);

        this.clubApi = new ClubApi();
    }

    render() {
        return (
            <div className={this.props.show ? "modal display-block" : "modal display-none"}>
                <div className="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">{this.props.photoData.title}</h5>
                            <button type="button" className="close" aria-label="Close" onClick={this.props.handleClose}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <Container className="modal-body">
                            <Row>
                                <Col>
                                    <img src={this.clubApi.baseUrl + "DownloadPhotoFile?StorageId=" + this.props.photoData.storageId} alt={this.props.photoData.title} />
                                </Col>
                            </Row>
                        </Container>
                        <div className="modal-footer">
                            <button className="btn btn-secondary" onClick={this.props.handleClose}>Close</button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }
}