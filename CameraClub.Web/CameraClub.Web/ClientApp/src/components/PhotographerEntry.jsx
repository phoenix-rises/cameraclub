import React, { Component } from 'react';
import { Container, Row, Col } from 'reactstrap';
import { PhotoEntry } from './PhotoEntry';

export class PhotographerEntry extends Component {
    render() {
        return (
            <>
                <Container key={this.props.photographer.id} className="card border-secondary top-margin-spacing">
                    <Row className="card-header">
                        <Col sm={1} className="col-form-label">
                            <button type="button" className="close" aria-label="Close" onClick={(e) => { e.preventDefault(); this.props.removePhotographer(this.props.photographer.id); }}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </Col>
                        <Col sm={6} className="col-form-label">
                            <h4 className="info">{this.props.photographer.firstName + " " + this.props.photographer.lastName}</h4>
                        </Col>
                        <Col sm={3} className="col-form-label">
                            Competition Number: {this.props.photographer.competitionNumber}
                        </Col>
                        <Col sm={2}>
                            <button className="btn btn-sm btn-secondary form-control" onClick={(e) => { e.preventDefault(); this.props.addPhoto(this.props.photographer.id); }}>Add Photo</button>
                        </Col>
                    </Row>
                    <Row className="card-body">
                        <Col>
                            <Container>
                                {this.props.photos.filter(p => p.photographerId === this.props.photographer.id && !p.isDeleted).map(photo =>
                                    <PhotoEntry key={photo.id} id={photo.id} categories={this.props.categories} title={photo.title} categoryId={photo.categoryId}
                                        handleTitleChange={this.props.handleTitleChange} handleCategoryChange={this.props.handleCategoryChange}
                                        uploadPhoto={(photoId) => { this.props.uploadPhoto(photoId); }} viewPhoto={(photoId) => { this.props.viewPhoto(photoId); }}
                                        removePhoto={(photoId) => { this.props.removePhoto(photoId); }} />
                                )}
                            </Container>
                        </Col>
                    </Row>
                </Container>
            </>
        );
    }
}