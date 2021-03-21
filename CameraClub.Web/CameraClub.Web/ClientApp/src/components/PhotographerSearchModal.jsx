import React, { Component } from 'react';
import { Container } from 'reactstrap';
import { PhotographerSearchBar } from './PhotographerSearchBar';
import { ClubApi } from '../ClubApi';
import { PhotographerSearchResults } from './PhotographerSearchResults';

export class PhotographerSearchModal extends Component {
    clubApi;

    constructor(props) {
        super(props);

        this.state = {
            photographers: [],
            loading: true,
            error: false,
            errorMessage: "",
            searchText: ""
        }

        this.clubApi = new ClubApi();

        this.updateSearch = this.updateSearch.bind(this);
        this.selectPhotographer = this.selectPhotographer.bind(this);
        this.loadState = this.loadState.bind(this);
    }

    componentDidMount() {
        this.clubApi.load("GetPhotographers", this.showError, this.loadState);
    }

    loadState(photographers) {
        this.setState({
            loading: false,
            error: false,
            errorMessage: null,
            photographers: photographers
        });
    }

    updateSearch(searchText) {
        this.setState({ searchText: searchText });
    }

    selectPhotographer(photographer) {
        this.props.resultChosen(photographer);
    }

    renderSearchModal() {
        return (
            <div className={this.props.show ? "modal display-block" : "modal display-none"}>
                <div className="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Find Photographer</h5>
                            <button type="button" className="close" aria-label="Close" onClick={this.props.handleClose}>
                                <span aria-hidden="true">&times;</span>
                            </button>
                        </div>
                        <Container className="modal-body">
                            <PhotographerSearchBar updateSearch={this.updateSearch} />
                            <PhotographerSearchResults photographers={this.state.photographers.filter(p => this.props.currentPhotographers.some(t => t.id === p.id && p.isDeleted) || this.props.currentPhotographers.every(s => s.id !== p.id))} searchText={this.state.searchText} selectPhotographer={this.selectPhotographer} />
                        </Container>
                        <div className="modal-footer">
                            <button className="btn btn-secondary" onClick={this.props.handleClose}>Close</button>
                        </div>
                    </div>
                </div>
            </div>
        );
    }

    render() {
        let contents = this.state.error
            ? <p>Error:  <span dangerouslySetInnerHTML={{ __html: this.state.errorMessage }}></span></p>
            : this.state.loading
                ? <p><em>Loading...</em></p>
                : this.renderSearchModal();

        return (
            <>
                {contents}
            </>
        );
    }
}