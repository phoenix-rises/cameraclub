import React, { Component } from 'react';
import { Container, Row, Col, Label } from 'reactstrap';
import { ClubApi } from '../ClubApi';

export class CompetitionPhotographers extends Component {
    clubApi;

    constructor(props) {
        super(props);

        this.clubApi = new ClubApi();

        var competitionId = props.match.params.competitionId;

        this.state = {
            competitionId: competitionId,
            photographerData: [],
            categories: [],
            error: false,
            loading: true,
            errorMessage: ""
        }

        this.loadPhotographerState = this.loadPhotographerState.bind(this);
        this.loadCategoryState = this.loadCategoryState.bind(this);
        this.showError = this.showError.bind(this);
        this.addPhoto = this.addPhoto.bind(this);
        this.removePhoto = this.removePhoto.bind(this);
        this.uploadPhoto = this.uploadPhoto.bind(this);
        this.addPhotographer = this.addPhotographer.bind(this);
        this.handleChange = this.handleChange.bind(this);
    }

    componentDidMount() {
        this.loadData();
    }

    loadData() {
        this.clubApi.load("GetCategories", this.showError, this.loadCategoryState);
    }

    loadCategoryState(categories) {
        this.setState({ "categories": categories });

        var test = {
            "competitionName": "Spring Extravaganza",
            "photographers":
                [
                    {
                        "id": "1", "firstName": "Bob", "lastName": "Barker", "competitionNumber": "1234",
                        "photos": [{ "id": 1, "title": "outdoor stuff", "categoryId": "1" },
                        { "id": 2, "title": "people stuff", "categoryId": "2" }]
                    },
                    {
                        "id": "2", "firstName": "Jeff", "lastName": "McGee", "competitionNumber": "1234",
                        "photos": [{ "id": 3, "title": "Jeff's photo", "categoryId": "1" },
                        { "id": 4, "title": "Jeff's second photo", "categoryId": "2" }]
                    }
                ]
        }

        this.loadPhotographerState(test); // TODO: replace this with api call below

        //this.clubApi.load("GetCompetitionEntries", this.showError, this.loadPhotographerState);
    }

    loadPhotographerState(photographers) {
        this.setState({
            loading: false,
            error: false,
            errorMessage: null,
            photographerData: photographers
        });
    }

    showError(error) {
        console.log(error);
        this.setState({
            loading: false,
            error: true,
            errorMessage: error
        });
    }

    addPhoto(photographerId) {
        // TODO: create photo row... think this just means add it to state
    }

    removePhoto(photo) {
        // TODO:
    }

    uploadPhoto(photo) {
        // TODO:
    }

    addPhotographer() {
        // TODO: update state for photographer
    }

    save() {
        // TODO: call save api
    }

    handleChange(event, photo) {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name = target.name;

        // TODO: figure out how to update the state correctly

        this.setState({
            [name]: value
        });
    }

    renderCompetitionPhotographers() {
        return (
            <>
                <Row>
                    <Col>
                        <h1 className="page-title">Entries for {this.state.photographerData.competitionName}</h1>
                    </Col>
                </Row>
                <Row>
                    <Col className="modal-header">
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.addPhotographer(); }}>Add Photographer</button>
                    </Col>
                </Row>
                <Row>
                    {this.state.photographerData.photographers.map(photographer =>
                        <Container key={photographer.id} className="bs-callout bs-callout-info">
                            <Row>
                                <Col>
                                    <h4 className="info">{photographer.firstName + " " + photographer.lastName}</h4>
                                </Col>
                                <Col>
                                    Competition Number: {photographer.competitionNumber}
                                </Col>
                                <Col>
                                    <button className="btn btn-secondary" onClick={(e) => { e.preventDefault(); this.addPhoto(photographer.id); }}>Add Photo</button>
                                </Col>
                            </Row>
                            <Row>
                                <Col>
                                    <Container>
                                        {this.state.photographerData.photographers.find(p => p.id === photographer.id).photos.map(photo =>
                                            <Row key={photo.id}>
                                                <Label for="title" sm={1}>Title</Label>
                                                <Col sm={4}>
                                                    <input type="text" name="title" placeholder="Title of photo" value={photo.title} onChange={(e) => { this.handleChange(e, photo); }} />
                                                </Col>
                                                <Col sm={3}>
                                                    <select value={photo.categoryId} onChange={(e) => { this.handleChange(e, photo); }}>
                                                        {this.state.categories.map(category =>
                                                            <option key={photo.id + " " + category.id} value={category.id}>
                                                                {category.name}
                                                            </option>
                                                        )}
                                                    </select>
                                                </Col>
                                                <Col sm={4}>
                                                    <button className="btn btn-sm btn-outline-primary" onClick={(e) => { e.preventDefault(); this.uploadPhoto(e, photo); }}>Upload</button>
                                                    <button className="btn btn-sm btn-outline-secondary" onClick={(e) => { e.preventDefault(); this.removePhoto(e, photo); }}>Remove</button>
                                                </Col>
                                            </Row>
                                        )}
                                    </Container>
                                </Col>
                            </Row>
                        </Container>
                    )}
                </Row>
                <Row>
                    <Col className="modal-footer">
                        <a className="btn btn-secondary" href={"/"}>Cancel</a>
                        <button className="btn btn-primary" onClick={(e) => { e.preventDefault(); this.save(); }}>Save Changes</button>
                    </Col>
                </Row>
            </>
        );
    }

    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderCompetitionPhotographers();

        return (
            <div>
                {contents}
            </div>
        );
    }
}